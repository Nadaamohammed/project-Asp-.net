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
    public class TourDestinationRepository(ApplicationDbContext context) : ITourDestinationRepository
    {
        private readonly ApplicationDbContext _context = context;

       
        public async Task<IEnumerable<TourDestination>> GetAllAsync()
        {
            return await _context.TourDestinations
                       .Include(x => x.Tour)
                       .Include(x => x.Destination)
                       .ToListAsync();
        }

        public async Task<TourDestination?> GetByIdAsync(int destinationId)
        {
            return await _context.TourDestinations
                                 .FirstOrDefaultAsync(td => td.DestinationId == destinationId);
        }

        public async Task AddAsync(TourDestination entity)
        {
            await _context.TourDestinations.AddAsync(entity);
        }
        public async Task DeleteAsync(int destinationId)
        {
            var entity = await GetByIdAsync(destinationId);
            if (entity != null)
            {
                _context.TourDestinations.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Destination>> GetDestinationsByTourAsync(int tourId)
        {
            return await _context.TourDestinations
                        .Where(td => td.TourId == tourId)
                        .Select(td => td.Destination)
                        .ToListAsync();
        }

        public async Task<IEnumerable<DomainLayer.Models.Tours.Tours>> GetToursByDestinationAsync(int destinationId)
        {
            return await _context.TourDestinations
                        .Where(td => td.DestinationId == destinationId)
                        .Select(td => td.Tour)
                        .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
