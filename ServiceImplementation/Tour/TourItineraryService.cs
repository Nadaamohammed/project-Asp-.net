using AutoMapper;
using DomainLayer.Models.Tours;
using DomainLayer.RepositoryInterface.Tours;
using ServiceAbstraction.Tour;
using Shared.Dto_s.Tour.TourItinerary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation.Tour
{
    public class TourItineraryService : ITourItineraryService
    {
        private readonly ITourItineraryRepository _repo;
        private readonly IMapper _mapper;

        public TourItineraryService(ITourItineraryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<TourItineraryDto>> GetByTourIdAsync(int tourId)
        {
            var data = await _repo.GetByTourIdAsync(tourId);
            return _mapper.Map<List<TourItineraryDto>>(data);
        }

        public async Task<TourItinerary?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<TourItineraryDto> CreateAsync(int tourId, CreateTourItineraryDto dto)
        {
            var entity = _mapper.Map<TourItinerary>(dto);
            entity.TourId = tourId;

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return _mapper.Map<TourItineraryDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTourItineraryDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
