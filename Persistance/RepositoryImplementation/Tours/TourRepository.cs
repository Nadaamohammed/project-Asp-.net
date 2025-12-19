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
    public class TourRepository : ITourRepository
    {
        private readonly ApplicationDbContext _context;

        public TourRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DomainLayer.Models.Tours.Tours>> GetAllAsync()
        {
            return await _context.Tours
                .Include(t => t.TourDestinations)
                .Include(t => t.AvailableDates)
                .ToListAsync();
        }

        public async Task<DomainLayer.Models.Tours.Tours?> GetByIdAsync(int id)
        {
            return await _context.Tours
                .Include(t => t.TourDestinations)
                .Include(t => t.AvailableDates)
                .Include(t => t.Itineraries)
                .Include(t => t.Inclusions)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<DomainLayer.Models.Tours.Tours>> GetFeaturedToursAsync()
        {
            return await _context.Tours
                .Where(t => t.BasePrice >= 300)
                .Take(6)
                .ToListAsync();
        }

        public async Task<IEnumerable<DomainLayer.Models.Tours.Tours>> SearchAsync(string? destination, decimal? minPrice, decimal? maxPrice, DateTime? startDate)
        {
            var query = _context.Tours.AsQueryable();

       

            if (minPrice.HasValue)
                query = query.Where(t => t.BasePrice >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(t => t.BasePrice <= maxPrice.Value);

            if (startDate.HasValue)
                query = query.Where(t => t.AvailableDates.Any(d => d.StartDate >= startDate.Value));

            return await query
                .Include(t => t.TourDestinations)
                .Include(t => t.AvailableDates)
                .ToListAsync();
        }

        public async Task AddAsync(DomainLayer.Models.Tours.Tours tour)
        {
            await _context.Tours.AddAsync(tour);
        }

        public void Update(DomainLayer.Models.Tours.Tours tour)
        {
            _context.Tours.Update(tour);
        }

        public void Delete(DomainLayer.Models.Tours.Tours tour)
        {
            _context.Tours.Remove(tour);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
