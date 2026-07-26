namespace TestTask.Models
{
    public class ResultEntity
    {
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public double TimeDelta { get; set; }

        public DateTime StartDate { get; set; }

        public double AverageExecutionTime { get; set; }

        public double AverageValue { get; set; }

        public double MedianValue { get; set; }

        public double MaxValue { get; set; }

        public double MinValue { get; set; }
    }
}
