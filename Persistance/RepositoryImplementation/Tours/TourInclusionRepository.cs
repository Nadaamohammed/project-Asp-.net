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
    public class TourInclusionRepository(ApplicationDbContext context) : ITourInclusionRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<TourInclusion>> GetByTourIdAsync(int tourId)
        {
            return await _context.TourInclusions
                        .Where(x => x.TourId == tourId)
                        .ToListAsync();
        }        
        public async Task<TourInclusion> GetByIdAsync(int id)
        {
            return await _context.TourInclusions.FindAsync(id);
        }

        public async Task AddAsync(TourInclusion inclusion)
        {
            await _context.TourInclusions.AddAsync(inclusion);
        }
        public async Task UpdateAsync(TourInclusion inclusion)
        {
            _context.TourInclusions.Update(inclusion);
        }
        public async Task DeleteAsync(TourInclusion inclusion)
        {
            _context.TourInclusions.Remove(inclusion);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
