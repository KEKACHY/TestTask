namespace TestTask.DTOs.Responses
{
    public class ResultResponseDto
    {
        public string FileName { get; set; } = null!;

        public double DeltaTime { get; set; }

        public DateTime StartDate { get; set; }

        public double AverageExecutionTime { get; set; }

        public double AverageValue { get; set; }

        public double MedianValue { get; set; }

        public double MaxValue { get; set; }

        public double MinValue { get; set; }
    }
}
