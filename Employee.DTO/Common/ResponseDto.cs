namespace Employee.DTO.Common
{
    public interface IResponseDto
    {
        bool IsPassed { get; set; }
        string Message { get; set; }
        List<string> Errors { get; set; }
        dynamic Data { get; set; }
    }
    public class ResponseDto : IResponseDto
    {
        public ResponseDto()
        {
            IsPassed = false;
            Message = "";
            Errors = new List<string>();
        }
        public bool IsPassed { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; } = null!;
    }
}
