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
    public class DestinationRepository(ApplicationDbContext context) : IDestinationRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Destination>> GetAllAsync()
        {
            return await _context.Destinations.ToListAsync();
        }

        public async Task<Destination?> GetByIdAsync(int id)
        {
            return await _context.Destinations.FindAsync(id);
        }
        public async Task AddAsync(Destination entity)
        {
            await _context.Destinations.AddAsync(entity);
        }
        public Task UpdateAsync(Destination entity)
        {
            _context.Destinations.Update(entity);
            return Task.CompletedTask;
        }
        public async Task DeleteAsync(int id)
        {
            var dest = await GetByIdAsync(id);
            if (dest != null)
            {
                _context.Destinations.Remove(dest);
            }
        }
        public async Task<List<Destination>> GetByTourIdAsync(int tourId)
        {
            return await _context.TourDestinations
                        .Where(t => t.TourId == tourId)
                        .Select(t => t.Destination)
                        .ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
