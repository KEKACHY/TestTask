using CsvHelper.Configuration.Attributes;

namespace TestTask.DTOs.Requests
{
    public class CsvValueDto
    {
        [Name("Date")]
        public DateTime Date { get; set; }


        [Name("ExecutionTime")]
        public double ExecutionTime { get; set; }


        [Name("Value")]
        public double Value { get; set; }
    }
}
