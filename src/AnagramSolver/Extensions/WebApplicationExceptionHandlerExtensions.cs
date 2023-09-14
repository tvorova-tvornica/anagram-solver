using AnagramSolver.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace AnagramSolver.Extensions;

public static class WebApplicationExceptionHandlerExtensions
{
    public static void UseAnagramSolverExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                // using static System.Net.Mime.MediaTypeNames;
                context.Response.ContentType = Text.Plain;

                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionHandlerPathFeature?.Error is BusinessRuleViolationException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync($"Business rule violation: {exceptionHandlerPathFeature.Error.Message}!");
                    return;
                }

                if (exceptionHandlerPathFeature?.Error is UniqueConstraintException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync($"Entity with the same unique key already exists!");
                    return;
                }

                if (exceptionHandlerPathFeature?.Error is not null)
                {
                    throw exceptionHandlerPathFeature.Error;
                }
            });
        });
    }
}
