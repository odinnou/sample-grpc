using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Server.Infrastructure.Exceptions;
using Server.Models;
using System.Net;

namespace Server.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> iLogger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> iLogger)
        {
            this.iLogger = iLogger;
        }

        public void OnException(ExceptionContext context)
        {
            int code = StatusCodes.Status500InternalServerError;

            double? errorCode = default;

            switch (context.Exception)
            {
                #region Status Code selon les exceptions
                case ProductNotFoundException _:
                    {
                        code = (int)HttpStatusCode.NotFound;
                    }
                    break;
                    #endregion
            }

            if (code == (int)HttpStatusCode.InternalServerError)
            {
                iLogger.LogError(context.Exception, "Not handled exception thrown");
            }
            else
            {
                iLogger.LogWarning(context.Exception, "Handled exception thrown");
            }

            context.Result = new ObjectResult(new ErrorResult(context.Exception.Message, errorCode, context.Exception.StackTrace));
            context.HttpContext.Response.StatusCode = code;

            context.ExceptionHandled = true;
        }
    }
}
