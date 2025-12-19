using AutoMapper;
using DomainLayer.Models.Tours;
using DomainLayer.RepositoryInterface.Tours;
using ServiceAbstraction.Tour;
using Shared.Dto_s.Tour.Destination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation.Tour
{
    public class DestinationService(IDestinationRepository repo, IMapper mapper) : IDestinationService
    {
        private readonly IDestinationRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<List<DestinationDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return _mapper.Map<List<DestinationDto>>(data);
        }
        public async Task<DestinationDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<DestinationDto>(entity);
        }
        public async Task<DestinationDto> CreateAsync(CreateDestinationDto dto)
        {
            var entity = _mapper.Map<Destination>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<DestinationDto>(entity);
        }
        public async Task<bool> UpdateAsync(int id, UpdateDestinationDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);

            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<List<DestinationDto>> GetByTourIdAsync(int tourId)
        {
            var data = await _repo.GetByTourIdAsync(tourId);
            return _mapper.Map<List<DestinationDto>>(data);
        }
    }
}
