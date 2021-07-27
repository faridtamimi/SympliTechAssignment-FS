using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SeoEvaluation.Api.DTOs;
using SeoEvaluation.Entities.Exceptions;

namespace SeoEvaluation.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppBaseException ex)
            {
                await SetResponseError(context, ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())
                    await SetResponseError(context, "Err_000", ex.Message, ex.StackTrace);
                else
                    await SetResponseError(context, "Err_000", "Internal Server Error");

            }
        }

        private static async Task SetResponseError(HttpContext context, string errorCode, string message = null, string detail = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            var response = new CustomException(errorCode, message, detail);

            var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}