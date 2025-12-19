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
    public class TourItineraryRepository : ITourItineraryRepository
    {
        private readonly ApplicationDbContext _context;

        public TourItineraryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TourItinerary>> GetByTourIdAsync(int tourId)
        {
            return await _context.TourItineraries
                .Where(x => x.TourId == tourId)
                .OrderBy(x => x.DayNumber)
                .ToListAsync();
        }

        public async Task<TourItinerary?> GetByIdAsync(int id)
        {
            return await _context.TourItineraries
                                 .FirstOrDefaultAsync(ti => ti.Id == id);
        }

        public async Task AddAsync(TourItinerary entity)
        {
            await _context.TourItineraries.AddAsync(entity);
        }

        public Task UpdateAsync(TourItinerary entity)
        {
            _context.TourItineraries.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.TourItineraries.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
