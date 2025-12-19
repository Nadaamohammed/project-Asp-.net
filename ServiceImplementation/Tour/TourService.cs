using AutoMapper;
using DomainLayer.Models.Tours;
using DomainLayer.RepositoryInterface.Tours;
using ServiceAbstraction.Tour;
using Shared.Dto_s.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation.Tour
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _repo;
        private readonly IMapper _mapper;

        public TourService(ITourRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TourDto>> GetAllAsync()
        {
            var tours = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<TourDto>>(tours);
        }

        public async Task<TourDto?> GetByIdAsync(int id)
        {
            var tour = await _repo.GetByIdAsync(id);
            return _mapper.Map<TourDto>(tour);
        }

        public async Task<IEnumerable<TourDto>> GetFeaturedToursAsync()
        {
            var tours = await _repo.GetFeaturedToursAsync();
            return _mapper.Map<IEnumerable<TourDto>>(tours);
        }

        public async Task<IEnumerable<TourDto>> SearchAsync(string? destination, decimal? minPrice, decimal? maxPrice, DateTime? startDate)
        {
            var tours = await _repo.SearchAsync(destination, minPrice, maxPrice, startDate);
            return _mapper.Map<IEnumerable<TourDto>>(tours);
        }

        public async Task<TourDto> CreateAsync(CreateTourDto dto)
        {
            var tour = _mapper.Map<Tours>(dto);
            await _repo.AddAsync(tour);
            await _repo.SaveChangesAsync();
            return _mapper.Map<TourDto>(tour);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTourDto dto)
        {
            var tour = await _repo.GetByIdAsync(id);
            if (tour == null) return false;

            _mapper.Map(dto, tour);
            _repo.Update(tour);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tour = await _repo.GetByIdAsync(id);
            if (tour == null) return false;

            _repo.Delete(tour);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
