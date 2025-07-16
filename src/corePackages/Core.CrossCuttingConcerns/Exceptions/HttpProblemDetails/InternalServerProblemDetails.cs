using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;

public class InternalServerProblemDetails : ProblemDetails
{
    public InternalServerProblemDetails(string detail)
    {
        Title = "Internal Server Error";
        Detail = "-";
        Status = StatusCodes.Status500InternalServerError;
        Type = "https://example.com/internal"; //Problem type URL can be customized
    }
}