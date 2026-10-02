using NooshApp.Api.Dtos;

namespace NooshApp.Api.Services.Interfaces
{
    //Controllers will call this service interface to execute the business logic 
    //rather than interacting with the database directly.
    public interface ICateringService
    {
        Task<CateringRequestDto> SubmitRequestAsync(CateringRequestCreateDto request);
        Task<CateringRequestDto?> GetByIdAsync(int id);
    }
}