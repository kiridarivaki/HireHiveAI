namespace HireHive.Application.Interfaces
{
    public interface IPiiRedactionService
    {
        Task<string> ExtractText(MemoryStream documentStream);
        Task<string> RedactPii(string documentText);
    }
}
