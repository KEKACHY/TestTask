namespace TestTask.DTOs.Responses
{
    public class UploadResultDto
    {
        public string FileName { get; set; } = null!;

        public int RowsProcessed { get; set; }

        public string Message { get; set; } = null!;
    }
}
