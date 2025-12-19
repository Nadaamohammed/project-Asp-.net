using AutoMapper;
using DomainLayer.Models.Booking_Transaction;
using DomainLayer.Models.Identity;
using DomainLayer.RepositoryInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceAbstraction;
using Shared.Dto_s.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation
{
    public class ReviewServices : IReviewServices
    {
        private readonly IGenericRepository<Review ,int> _reviewRepo;
        IGenericRepository<DomainLayer.Models.Booking_Transaction.Booking, int> _bookingRepo;
        private readonly IMapper _mapper;

        public ReviewServices(IGenericRepository<Review ,int> reviewRepo , IGenericRepository<DomainLayer.Models.Booking_Transaction.Booking, int> bookingRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _bookingRepo = bookingRepo;
            _mapper = mapper;
        }

        // إضافة Review جديد
        public async Task<ReviewDto> AddReviewAsync(CreateReviewDto dto)

        {
            //bool hasBooking = await UserHasBooking(dto.UserId, dto.TargetType, dto.TargetId);

            //if (!hasBooking)
            //    throw new Exception("User cannot review this item because they haven't booked it.");
            // تحويل DTO → Review Model
            var review = _mapper.Map<Review>(dto);
            review.ReviewDate = DateTime.UtcNow;
            
            await _reviewRepo.Add(review);
            await _reviewRepo.SaveChangesAsync();

            return _mapper.Map<ReviewDto>(review);
        }

        //// تعديل Review
        //[Authorize]
        //public async Task<bool> UpdateReview(int reviewId, UpdateReviewDto dto, string userId, bool isAdmin)
        //{
        //    var review = await _reviewRepo.GetByIdAsync(reviewId);
        //    if (review == null)
        //        return false;


        //    // Authorization:
        //    if (review.UserId != userId && !isAdmin)
        //        throw new UnauthorizedAccessException("You cannot update this review.");

        //    // Update باستخدام AutoMapper
        //    _mapper.Map(dto, review);

        //    review.ReviewDate = DateTime.UtcNow;

        //    _reviewRepo.Update(review);
        //    await _reviewRepo.SaveChangesAsync();

        //    return true;
        //}

        public async Task<ReviewDto?> UpdateReviewAsync(int reviewId, UpdateReviewDto dto, string userId, bool isAdmin)
        {
            var review = await _reviewRepo.GetByIdAsync(reviewId);
            if (review == null)
                return null;

            // Authorization:
            if (review.UserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("You cannot update this review.");

            // Update باستخدام AutoMapper
            _mapper.Map(dto, review);
            review.ReviewDate = DateTime.UtcNow;

            _reviewRepo.Update(review);
            await _reviewRepo.SaveChangesAsync();

           
            return _mapper.Map<ReviewDto>(review);
        }


        // حذف Review

        public async Task<bool> DeleteReview(int reviewId, string currentUserId, bool isAdmin)
        {
            var review = await _reviewRepo.GetByIdAsync(reviewId);
            if (review == null)
                return false;

            // Authorization:
            if (review.UserId != currentUserId && !isAdmin)
                throw new UnauthorizedAccessException("You cannot delete this review.");

            _reviewRepo.Delete(review);
            await _reviewRepo.SaveChangesAsync();
            return true;
        }

        // جلب كل الريڤيوز حسب (Hotel – Tour – Flight)
        public async Task<IEnumerable<ReviewDto>> GetReviewsAsync(string targetType)
        {
            var reviews = (await _reviewRepo.GetAllAsync())
                //.Where(r =>
                //    r.TargetType.ToString().Equals(targetType, StringComparison.OrdinalIgnoreCase)
                //    && r.TargetId == targetId)
                .ToList();

            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }



        private async Task<bool> UserHasBooking(string userId, string targetType, int targetId)
        {
            var bookings = await _bookingRepo.GetAllAsync();

            foreach (var booking in bookings.Where(b => b.UserId == userId))
            {
                if (targetType == "Hotel" && booking.HotelBooking.Room.HotelId == targetId)
                    return true;

                if (targetType == "Tour" && booking.TourBooking.TourDate.TourId == targetId)
                    return true;

                if (targetType == "Flight" && booking.FlightBooking?.FlightId == targetId)
                    return true;
            }

            return false;
        }

    }


}
