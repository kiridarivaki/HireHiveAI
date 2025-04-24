using HireHive.Common.Helpers;
using HireHive.Domain.Exceptions;
using HireHive.Infrastructure.Results;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HireHive.Infrastructure.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    private static readonly JsonSerializerOptions SerializeOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        var statusCode = response.StatusCode;

        var errorResult = new ErrorResult();

        try
        {
            await next(context);

            switch (statusCode)
            {
                //401 - Unauthorized
                case StatusCodes.Status401Unauthorized:
                //403 - Forbidden
                case StatusCodes.Status403Forbidden:
                    errorResult.ErrorMessage = GetErrorMessage(statusCode.ToString());
                    break;
            }

            if (statusCode != StatusCodes.Status200OK)
            {
                var body = JsonSerializer.Serialize(errorResult, SerializeOptions);
                await response.WriteAsync(body);
            }
        }
        catch (Exception exception)
        {
            var exceptionName = exception.GetType().Name;
            int errorCode;

            switch (exception)
            {
                //400 - BadRequest
                //404 - NotFound
                case BaseException e:
                    errorCode = e.Type switch
                    {
                        ExceptionType.Default => StatusCodes.Status400BadRequest,
                        ExceptionType.NotFound => StatusCodes.Status404NotFound,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    errorResult.ErrorCode = errorCode;

                    errorResult.ErrorMessage = string.IsNullOrWhiteSpace(e.Message)
                        ? GetErrorMessage(exceptionName)
                        : e.Message;

                    response.StatusCode = errorCode;
                    break;

                //500 - InternalServerError
                default:
                    errorCode = StatusCodes.Status500InternalServerError;
                    errorResult.ErrorCode = errorCode;
                    errorResult.ErrorMessage = GetErrorMessage(errorCode.ToString());
                    response.StatusCode = errorCode;
                    break;
            }

            var body = JsonSerializer.Serialize(errorResult, SerializeOptions);
            await response.WriteAsync(body);
        }
    }

    private static string GetErrorMessage(string key)
    {
        return ResourceManagerHelper.GetString(fileName: "Exceptions", key: key);
    }
}
