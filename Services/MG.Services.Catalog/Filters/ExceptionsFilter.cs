using MG.Services.Catalog.Domain;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MG.Services.Catalog.Filters
{
    public class ExceptionsFilter : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ItemNotFoundException)
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 404;
            }
        }
    }
}
