using Microsoft.AspNetCore.Mvc;
using TestTask.DTOs.Requests;
using TestTask.Exceptions;
using TestTask.Services;
using TestTask.Services.Interfaces;

namespace TestTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly ICsvProcessingService _csvProcessingService;
    private readonly IResultService _resultService;


    public ResultsController(
        ICsvProcessingService csvProcessingService,
        IResultService resultService)
    {
        _csvProcessingService = csvProcessingService;
        _resultService = resultService;
    }


    /// <summary>
    /// Загрузка и обработка CSV файла
    /// </summary>
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(
        [FromForm] UploadCsvRequest request)
    {
        try
        {
            var result =
                await _csvProcessingService.ProcessAsync(
                    request.File);


            return Ok(result);
        }
        catch (InvalidCsvException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    /// <summary>
    /// Получение результатов с фильтрами
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetResults(
        [FromQuery] ResultFilterRequest filter)
    {
        var result =
            await _resultService.GetResultsAsync(filter);


        return Ok(result);
    }

    /// <summary>
    /// Получение последних 10 значений файла
    /// </summary>
    [HttpGet("{fileName}/values")]
    public async Task<IActionResult> GetLastValues(
        string fileName)
    {
        var result =
            await _resultService.GetLastValuesAsync(
                fileName);


        return Ok(result);
    }
}