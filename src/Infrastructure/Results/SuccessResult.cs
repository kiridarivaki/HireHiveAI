using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Infrastructure.Results;

public class SuccessResult : OkObjectResult
{
    private static int Status => StatusCodes.Status200OK;
    private static string Message => "Success";

    public SuccessResult() : base(new { Status, Message })
    { }
}
