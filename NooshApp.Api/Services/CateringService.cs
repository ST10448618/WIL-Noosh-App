using NooshApp.Api.Dtos;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Services
{
    //Business logic implementation for handling catering requests
    public class CateringService : ICateringService
    {
        private readonly ICateringRepository _cateringRepository;
        public CateringService(ICateringRepository cateringRepository) { _cateringRepository = cateringRepository; }

        public async Task<CateringRequestDto> SubmitRequestAsync(CateringRequestCreateDto request)
        {
            var entity = new CateringRequest
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                EventDate = request.EventDate,
                GuestCount = request.GuestCount,
                EventLocation = request.EventLocation,
                AdditionalNotes = request.AdditionalNotes,
                Status = CateringStatus.New, // Automatically set the initial status to "New"
                SubmittedAt = DateTime.UtcNow
            };

            await _cateringRepository.AddAsync(entity);
            return ToDto(entity);
        }
// Asynchronously retrieves a catering request by its unique ID
        public async Task<CateringRequestDto?> GetByIdAsync(int id)
        {
            var entity = await _cateringRepository.GetByIdAsync(id);
            return entity == null ? null : ToDto(entity);
        }

        private static CateringRequestDto ToDto(CateringRequest entity) => new CateringRequestDto
        {
            Id = entity.Id,
            FullName = entity.FullName,
            EventDate = entity.EventDate,
            GuestCount = entity.GuestCount,
            EventLocation = entity.EventLocation,
            AdditionalNotes = entity.AdditionalNotes,
            Status = entity.Status.ToString(),
            SubmittedAt = entity.SubmittedAt
        };
    }
}