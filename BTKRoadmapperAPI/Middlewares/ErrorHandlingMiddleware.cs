using BTKRoadmapperAPI.DTOs;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Net;

namespace BTKRoadmapperAPI.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next,  ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            var code = HttpStatusCode.InternalServerError;
            var response = new object();
            var messageKey = "DefaultError";

            if (exception is UserFriendlyException userFriendlyException)
            {
                code = HttpStatusCode.BadRequest;
                response = Response<object>.Fail(userFriendlyException.Message, (int)code);
            }
            else if (exception is UnauthorizedAccessException unauthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
                response = Response<object>.Fail(unauthorizedAccessException.Message, (int)code);
            }
            else if (exception is ClientBreakFlow clientBreakFlow)
            {
                code = HttpStatusCode.BadRequest;
                var clienterrorMessage = clientBreakFlow.Message;
                response = Response<object>.Fail(clienterrorMessage, (int)code);
            }
            else
            {
                //var errorMessage = _localizer[messageKey]; Burası olması gereke ntest ortamında kapattık
                var errorMessage = exception.Message + exception.InnerException;
                response = Response<object>.Fail(errorMessage, (int)code);
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var result = JsonConvert.SerializeObject(response);
            _logger.LogError(exception.ToString());
            return context.Response.WriteAsync(result);
        }

    }
}
