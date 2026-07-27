using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.DTOs.Requests;
using TestTask.DTOs.Responses;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class ResultService : IResultService
    {
        private readonly AppDbContext _context;


        public ResultService(
            AppDbContext context)
        {
            _context = context;
        }



        public async Task<List<ResultResponseDto>> GetResultsAsync(
            ResultFilterRequest filter)
        {
            var query = _context.Results
                .AsQueryable();



            if (!string.IsNullOrWhiteSpace(filter.FileName))
            {
                query = query.Where(x =>
                    x.FileName == filter.FileName);
            }



            if (filter.StartDateFrom.HasValue)
            {
                query = query.Where(x =>
                    x.StartDate >= filter.StartDateFrom.Value);
            }



            if (filter.StartDateTo.HasValue)
            {
                query = query.Where(x =>
                    x.StartDate <= filter.StartDateTo.Value);
            }



            if (filter.AverageValueFrom.HasValue)
            {
                query = query.Where(x =>
                    x.AverageValue >= filter.AverageValueFrom.Value);
            }



            if (filter.AverageValueTo.HasValue)
            {
                query = query.Where(x =>
                    x.AverageValue <= filter.AverageValueTo.Value);
            }



            if (filter.AverageExecutionTimeFrom.HasValue)
            {
                query = query.Where(x =>
                    x.AverageExecutionTime >= filter.AverageExecutionTimeFrom.Value);
            }



            if (filter.AverageExecutionTimeTo.HasValue)
            {
                query = query.Where(x =>
                    x.AverageExecutionTime <= filter.AverageExecutionTimeTo.Value);
            }



            return await query
                .Select(x => new ResultResponseDto
                {
                    FileName = x.FileName,

                    DeltaTime = x.TimeDelta,

                    StartDate = x.StartDate,

                    AverageExecutionTime =
                        x.AverageExecutionTime,

                    AverageValue =
                        x.AverageValue,

                    MedianValue =
                        x.MedianValue,

                    MaxValue =
                        x.MaxValue,

                    MinValue =
                        x.MinValue
                })
                .ToListAsync();
        }

        public async Task<List<ValueResponseDto>> GetLastValuesAsync(
        string fileName)
        {
            return await _context.Values
                .Where(x => x.FileName == fileName)

                .OrderByDescending(x => x.Date)

                .Take(10)

                .Select(x => new ValueResponseDto
                {
                    Date = x.Date,

                    ExecutionTime =
                        x.ExecutionTime,

                    Value =
                        x.Value
                })

                .ToListAsync();
        }
    }
}
