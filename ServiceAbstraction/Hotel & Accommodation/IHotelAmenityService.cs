
using Shared.Dto_s.Hotel___Accommodation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceAbstraction.Hotel___Accommodation
{
    public interface IHotelAmenityService
    {
       
        Task<IEnumerable<HotelAmenityDto>> GetAllAsync();

        Task<HotelAmenityDto> GetByIdAsync(int hotelId, int amenityId);

        Task<IEnumerable<HotelAmenityDto>> GetByHotelAsync(int hotelId);
        Task<HotelAmenityDto> AddAsync(HotelAmenityDto dto);
        Task<bool> DeleteAsync(int hotelId, int amenityId);
    }
}


