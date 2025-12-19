using AutoMapper;
using DomainLayer.Models.Tours;
using DomainLayer.RepositoryInterface.Tours;
using ServiceAbstraction.Tour;
using Shared.Dto_s.Tour.TourDestination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation.Tour
{
    public class TourDestinationService(ITourDestinationRepository repo, IMapper mapper) : ITourDestinationService
    {
        private readonly ITourDestinationRepository _repo = repo;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<TourDestinationDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<TourDestinationDto>>(data);
        }

        public async Task<TourDestination?> GetByIdAsync(int destinationId)
        {
            return await _repo.GetByIdAsync(destinationId);
        }

        public async Task<bool> AddAsync(AddTourDestinationDto dto)
        {
            var exist = await _repo.GetByIdAsync(dto.DestinationId);
            if (exist != null) return false;

            var entity = _mapper.Map<TourDestination>(dto);

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(int destinationId)
        {
            await _repo.DeleteAsync(destinationId);
        }


        public async Task<IEnumerable<Destination>> GetDestinationsByTourAsync(int tourId)
        {
            return await _repo.GetDestinationsByTourAsync(tourId);
        }

        public async Task<IEnumerable<Tours>> GetToursByDestinationAsync(int destinationId)
        {
            return await _repo.GetToursByDestinationAsync(destinationId);
        }
    }
}
