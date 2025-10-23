using System.Net;
using System.Text.Json;
using ECommerce.ShareLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ShareLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //Declare default variables
            string message = "Sorry, internal server error occurred. Kindly try again";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await next(context);

                //check if Response is Too Many Requests // 429 status code
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many requests. Please try again later.";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }

                //If Response is UnAuthorzied  // 401 status code
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You are not authorized to access this resource.";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, title, message, statusCode);
                }

                //If Response is Forbidden  // 403 status code
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    message = "You are not allowed/required to access";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                //Log Original Exceptions / File, Debugger, Console
                LogException.LogExceptions(ex);

                //Check if Exception is Timeout // 408 request timeout
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time";
                    message = "Request timeout... Try again";
                    statusCode = (int)StatusCodes.Status408RequestTimeout;
                }

                //If Exception is caught 
                //If none of the exceptions then do the default 
                await ModifyHeader(context, title, message, statusCode);

            }
        }

        private async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            //display scary-free message to client
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = title
            }), CancellationToken.None);
            return;
        }
    }
}
