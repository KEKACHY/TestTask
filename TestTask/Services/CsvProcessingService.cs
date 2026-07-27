using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TestTask.Data;
using TestTask.DTOs.Requests;
using TestTask.DTOs.Responses;
using TestTask.Exceptions;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class CsvProcessingService : ICsvProcessingService
    {
        private readonly AppDbContext _context;

        public CsvProcessingService(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<UploadResultDto> ProcessAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new InvalidCsvException(
                    "Файл пустой");
            }


            var fileName = file.FileName;


            using var stream = file.OpenReadStream();


            var rows = ParseCsv(stream);


            ValidateRows(rows);

            await using var transaction =
                        await _context.Database.BeginTransactionAsync();


            try
            {
                var oldValues = await _context.Values
                    .Where(x => x.FileName == fileName)
                    .ToListAsync();


                _context.Values.RemoveRange(oldValues);

                var oldResult = await _context.Results
                    .FirstOrDefaultAsync(
                        x => x.FileName == fileName);


                if (oldResult != null)
                {
                    _context.Results.Remove(oldResult);
                }


                await _context.SaveChangesAsync();

                var valueEntities = rows
                    .Select(x => new ValueEntity
                    {
                        FileName = fileName,

                        Date = x.Date,

                        ExecutionTime = x.ExecutionTime,

                        Value = x.Value
                    })
                    .ToList();


                await _context.Values.AddRangeAsync(valueEntities);


                await _context.SaveChangesAsync();

                var resultEntity = CreateResult(
                    fileName,
                    rows);


                await _context.Results.AddAsync(
                    resultEntity);


                await _context.SaveChangesAsync();


                await transaction.CommitAsync();


                return new UploadResultDto
                {
                    FileName = fileName,

                    RowsProcessed = rows.Count,

                    Message = "Файл успешно обработан"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                Console.WriteLine(ex.InnerException?.Message);

                throw;
            }
        }

        private List<CsvValueDto> ParseCsv(Stream stream)
        {
            using var reader = new StreamReader(stream);


            var config = new CsvConfiguration(
                CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true
            };


            using var csv = new CsvReader(
                reader,
                config);


            var rows = csv
                .GetRecords<CsvValueDto>()
                .ToList();


            foreach (var row in rows)
            {
                row.Date = DateTime.SpecifyKind(
                    row.Date,
                    DateTimeKind.Utc);
            }


            return rows;
        }

        private void ValidateRows(List<CsvValueDto> rows)
        {
            if (rows.Count < 1)
            {
                throw new InvalidCsvException(
                    "Файл не содержит строк");
            }


            if (rows.Count > 10000)
            {
                throw new InvalidCsvException(
                    "Количество строк больше 10000");
            }


            var minDate =
                new DateTime(2000, 1, 1);



            foreach (var row in rows)
            {
                if (row.Date < minDate)
                {
                    throw new InvalidCsvException(
                        "Дата не может быть раньше 01.01.2000");
                }


                if (row.Date > DateTime.UtcNow)
                {
                    throw new InvalidCsvException(
                        "Дата не может быть позже текущего времени");
                }


                if (row.ExecutionTime < 0)
                {
                    throw new InvalidCsvException(
                        "ExecutionTime не может быть меньше 0");
                }


                if (row.Value < 0)
                {
                    throw new InvalidCsvException(
                        "Value не может быть меньше 0");
                }
            }
        }
        private ResultEntity CreateResult(string fileName, List<CsvValueDto> rows)
        {
            var minDate =
                rows.Min(x => x.Date);


            var maxDate =
                rows.Max(x => x.Date);



            var orderedValues =
                rows
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToList();



            double medianValue;


            if (orderedValues.Count % 2 == 0)
            {
                medianValue =
                    (orderedValues[orderedValues.Count / 2 - 1]
                    +
                    orderedValues[orderedValues.Count / 2])
                    / 2;
            }
            else
            {
                medianValue =
                    orderedValues[orderedValues.Count / 2];
            }



            return new ResultEntity
            {
                FileName = fileName,

                TimeDelta =
                    (maxDate - minDate)
                    .TotalSeconds,

                StartDate = minDate,

                AverageExecutionTime =
                    rows.Average(x => x.ExecutionTime),

                AverageValue =
                    rows.Average(x => x.Value),

                MedianValue =
                    medianValue,

                MaxValue =
                    rows.Max(x => x.Value),

                MinValue =
                    rows.Min(x => x.Value)
            };
        }
    }
}
