using TestTask.DTOs.Responses;

namespace TestTask.Services.Interfaces
{
    public interface ICsvProcessingService
    {
        Task<UploadResultDto> ProcessAsync(
            IFormFile file);
    }
}
