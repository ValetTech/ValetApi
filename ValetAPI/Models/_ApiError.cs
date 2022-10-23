using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValetAPI.Models;

public class ApiError
{
    public ApiError()
    {
    }

    public ApiError(string message)
    {
        Message = message;
    }

    public ApiError(ModelStateDictionary modelState)
    {
        Message = "Invalid parameters.";
        Detail = modelState
            .FirstOrDefault(x => x.Value.Errors.Any()).Value.Errors
            .FirstOrDefault().ErrorMessage;
    }

    public string Message { get; set; }

    public string Detail { get; set; }
}