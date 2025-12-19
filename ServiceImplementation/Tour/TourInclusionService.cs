using AutoMapper;
using DomainLayer.Models.Tours;
using DomainLayer.RepositoryInterface.Tours;
using ServiceAbstraction.Tour;
using Shared.Dto_s.Tour.TourInclusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation.Tour
{
    public class TourInclusionService(ITourInclusionRepository repo, IMapper mapper) : ITourInclusionService
    {
        private readonly ITourInclusionRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<TourInclusionDto>> GetByTourIdAsync(int tourId)
        {
            var result = await _repo.GetByTourIdAsync(tourId);
            return _mapper.Map<IEnumerable<TourInclusionDto>>(result);
        }
        public async Task<TourInclusionDto> GetByIdAsync(int id)
        {
            var inc = await _repo.GetByIdAsync(id);
            return _mapper.Map<TourInclusionDto>(inc);
        }
        public async Task<TourInclusionDto> CreateAsync(int tourId, CreateTourInclusionDto dto)
        {
            var inclusion = _mapper.Map<TourInclusion>(dto);
            inclusion.TourId = tourId;

            await _repo.AddAsync(inclusion);
            await _repo.SaveChangesAsync();

            return _mapper.Map<TourInclusionDto>(inclusion);
        }

        public async Task<TourInclusionDto> UpdateAsync(int id, UpdateTourInclusionDto dto)
        {
            var inclusion = await _repo.GetByIdAsync(id);
            if (inclusion == null) return null;

            _mapper.Map(dto, inclusion);

            await _repo.UpdateAsync(inclusion);
            await _repo.SaveChangesAsync();

            return _mapper.Map<TourInclusionDto>(inclusion);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var inclusion = await _repo.GetByIdAsync(id);
            if (inclusion == null) return false;

            await _repo.DeleteAsync(inclusion);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
