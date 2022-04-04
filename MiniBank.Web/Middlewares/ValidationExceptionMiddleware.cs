﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MiniBank.Core.Exceptions;
using MiniBank.Data.Exceptions;

namespace MiniBank.Web.Middlewares
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new {Error = $"{validationException.Message}"});
            }
            catch (FluentValidation.ValidationException exception)
            {
                var errors = exception.Errors
                    .Select(error => $"{error.PropertyName}: {error.ErrorMessage}");

                var errorMessage = string.Join(Environment.NewLine, errors);

                await httpContext.Response.WriteAsJsonAsync(new {Error = $"{errorMessage}"});
            }
            catch (ObjectNotFoundException objectNotFoundException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(new {Error = $"{objectNotFoundException.Message}"});
            }
        }
    }
}