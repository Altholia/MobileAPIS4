using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Http;

namespace MobileAPIS4.Helpers;

public class CustomErrorMessage
{
    private static string _errorTypeString;
    public static object CreateErrorMessage(int httpStatusCode, string errorMessage)
    {
        switch (httpStatusCode)
        {
            case 400:
                _errorTypeString = ErrorStatusCode.BadRequest.ToString();
                break;
            case 404:
                _errorTypeString=ErrorStatusCode.NotFound.ToString();
                break;
        }

        var error = new
        {
            errorType = _errorTypeString,
            httpStatusCode,
            errorMessage
        };

        return error;
    }
}