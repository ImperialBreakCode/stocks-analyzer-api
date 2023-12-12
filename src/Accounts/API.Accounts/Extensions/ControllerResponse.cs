using API.Accounts.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Accounts.Extensions
{
    public static class ControllerResponse
    {
        public static IActionResult ParseAndReturnMessage(this ControllerBase controller, string message)
        {
            ResponseType responseType = ResponseParser.ParseResponseMessage(message);

            switch (responseType)
            {
                case ResponseType.NotFound:
                    return controller.NotFound(message);
                case ResponseType.Unauthorized:
                    return controller.Unauthorized(message);
                case ResponseType.Conflict:
                    return controller.Conflict(message);
                case ResponseType.BadRequest:
                    return controller.BadRequest(message);
                default:
                    return controller.Ok(message);
            }
        }
    }
}
