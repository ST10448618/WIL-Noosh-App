using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public interface ICateringApiClient
    {
        Task<CateringRequestDto> SubmitAsync(CateringRequestCreateDto request);
        Task<CateringRequestDto?> GetByIdAsync(int id);
    }
}