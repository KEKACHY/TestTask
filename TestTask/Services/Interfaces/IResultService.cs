using TestTask.DTOs.Requests;
using TestTask.DTOs.Responses;

namespace TestTask.Services.Interfaces
{
    public interface IResultService
    {
        Task<List<ResultResponseDto>> GetResultsAsync(
            ResultFilterRequest filter);

        Task<List<ValueResponseDto>> GetLastValuesAsync(
        string fileName);
    }
}
