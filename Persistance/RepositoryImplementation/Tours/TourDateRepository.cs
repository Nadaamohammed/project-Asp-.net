using DomainLayer.Models.Tours;
using DomainLayer.RepositoryInterface.Tours;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.RepositoryImplementation.Tours
{
    public class TourDateRepository(ApplicationDbContext context) : ITourDateRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<TourDate>> GetAllAsync()
            => await _context.TourDates.ToListAsync();

        public async Task<TourDate> GetByIdAsync(int id) 
            => await _context.TourDates.FindAsync(id);
        public async Task<IEnumerable<TourDate>> GetByTourIdAsync(int tourId) 
            => await _context.TourDates.Where(x => x.TourId == tourId).ToListAsync();
        public async Task<TourDate> AddAsync(TourDate tourDate)
        {
            _context.TourDates.Add(tourDate);
            await _context.SaveChangesAsync();
            return tourDate;
        }
        public async Task UpdateAsync(TourDate tourDate)
        {
            _context.TourDates.Update(tourDate);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var item = await _context.TourDates.FindAsync(id);
            if (item != null)
            {
                _context.TourDates.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<TourDate>> GetUpcomingAsync()
            => await _context.TourDates
            .Where(x => x.StartDate >= DateTime.Now)
            .OrderBy(x => x.StartDate)
            .ToListAsync();
        public async Task<IEnumerable<TourDate>> FilterAsync(decimal? minPrice, decimal? maxPrice, DateTime? startDate)
        {
            var query = _context.TourDates.AsQueryable();

            if (minPrice.HasValue)
                query = query.Where(x => x.Price >= minPrice);

            if (maxPrice.HasValue)
                query = query.Where(x => x.Price <= maxPrice);

            if (startDate.HasValue)
                query = query.Where(x => x.StartDate >= startDate);

            return await query.ToListAsync();
        }
        public async Task<bool> CheckAvailabilityAsync(int id)
        {
            var result = await _context.TourDates.FindAsync(id);
            return result != null && result.AvailableSeats > 0;
        }
        
    }
}
