using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dto_s.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // ============================
        // Create Booking
        // ===========================

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            Console.WriteLine(User.Identity.IsAuthenticated);
            Console.WriteLine(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var bookingDetails = await _bookingService.CreateAsync(userId, dto);

            // Return 200 مع تفاصيل الحجز
            return Ok(bookingDetails);
        }

        // ============================
        // Get Booking By Id
        // ============================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookingService.GetByIdAsync(id);
            return Ok(result);
        }

        // ============================
        // Get My Bookings
        // ============================
        [HttpGet("my-bookings")]
        //[Authorize] 
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var bookings = await _bookingService.GetUserBookingsAsync(userId);

            if (bookings == null || !bookings.Any())
                return NotFound("No bookings found for this user.");

            return Ok(new
            {
                Count = bookings.Count(),
                Data = bookings
            });
        }

        // ============================
        // Update Booking
        // ============================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookingDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _bookingService.UpdateAsync(id, userId, dto);

            return Ok(result);
        }

        // ============================
        // Cancel Booking
        // ============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var success = await _bookingService.CancelAsync(id, userId);

            if (!success)
                return NotFound();

            return Ok(new { message = "Booking cancelled successfully" });
        }
    }

}
