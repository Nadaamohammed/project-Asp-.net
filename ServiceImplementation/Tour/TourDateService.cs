using AutoMapper;
using DomainLayer.Models.Tours;
using DomainLayer.RepositoryInterface.Tours;
using ServiceAbstraction.Tour;
using Shared.Dto_s.Tour.TourDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation.Tour
{
    public class TourDateService(ITourDateRepository repo, IMapper mapper) : ITourDateService
    {
        private readonly ITourDateRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<TourDateDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<TourDateDto>>(items);
        }

        public async Task<TourDateDto> GetByIdAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return _mapper.Map<TourDateDto>(item);
        }

        public async Task<IEnumerable<TourDateDto>> GetByTourIdAsync(int tourId)
        {
            var items = await _repo.GetByTourIdAsync(tourId);
            return _mapper.Map<IEnumerable<TourDateDto>>(items);
        }
        public async Task<TourDateDto> CreateAsync(CreateTourDateDto dto)
        {
            var entity = _mapper.Map<TourDate>(dto);
            await _repo.AddAsync(entity);

            return _mapper.Map<TourDateDto>(entity);
        }
        public async Task UpdateAsync(int id, UpdateTourDateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return;

            _mapper.Map(dto, entity);

            await _repo.UpdateAsync(entity);
        }
        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
        public async Task<IEnumerable<TourDateDto>> GetUpcomingAsync()
        {
            var items = await _repo.GetUpcomingAsync();
            return _mapper.Map<IEnumerable<TourDateDto>>(items);
        }
        public async Task<IEnumerable<TourDateDto>> FilterAsync(decimal? minPrice, decimal? maxPrice, DateTime? startDate)
        {
            var items = await _repo.FilterAsync(minPrice, maxPrice, startDate);
            return _mapper.Map<IEnumerable<TourDateDto>>(items);
        }

        public async Task<bool> CheckAvailabilityAsync(int id)
        {
            return await _repo.CheckAvailabilityAsync(id);
        }
    }
}
