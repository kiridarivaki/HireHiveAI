namespace HireHive.Application.Interfaces
{
    public interface IPiiRedactionService
    {
        Task<string> ExtractBlobText(string blobUri);
        Task RedactPii(string blobUri);
    }
}
