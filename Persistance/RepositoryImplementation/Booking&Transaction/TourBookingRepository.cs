using DomainLayer.Models.Booking_Transaction;
using DomainLayer.RepositoryInterface.Booking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.RepositoryImplementation.Booking_Transaction
{
    public class TourBookingRepository : ITourBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public TourBookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TourBooking> CreateAsync(TourBooking tourBooking)
        {
            _context.TourBookings.Add(tourBooking);
            await _context.SaveChangesAsync();
            return tourBooking;
        }
        public async Task<TourBooking?> GetByBookingIdAsync(int bookingId)
        {
            return await _context.TourBookings
                       .Include(t => t.TourDate)
                       .FirstOrDefaultAsync(t => t.BookingId == bookingId);
        }
        public async Task<TourBooking?> GetBookingTourByIdAsync(int bookingId)
        {
            return await _context.TourBookings
                       .Include(tb => tb.TourDate)
                           .ThenInclude(td => td.Tour)
                       .FirstOrDefaultAsync(tb => tb.BookingId == bookingId);
        }
        public async Task<bool> UpdateAsync(TourBooking tourBooking)
        {
            _context.TourBookings.Update(tourBooking);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int bookingId)
        {
            var booking = await _context.TourBookings.FindAsync(bookingId);
            if (booking == null) return false;

            _context.TourBookings.Remove(booking);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
