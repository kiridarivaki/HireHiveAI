namespace HireHive.Infrastructure.Results
{
    public class ErrorResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = null!;
    }
}
