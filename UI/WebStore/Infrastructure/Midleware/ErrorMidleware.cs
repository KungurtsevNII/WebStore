using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Midleware
{
    public class ErrorMidleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ErrorMidleware> _Looger;

        public ErrorMidleware(RequestDelegate Next, ILogger<ErrorMidleware> Looger)
        {
            _Next = Next;
            _Looger = Looger;
        }

        public async Task Invoke(HttpContext Context)
        {
            try
            {
                // Может обработать перед тем как отдал
                await _Next(Context);
                // Может обработать после того как вернули
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(Context, error);
                throw;
            }
        }

        private Task HandleExceptionAsync(HttpContext Context, Exception Error)
        {
            _Looger.LogError(Error, "Ошибка при обработке запроса {0}", Context.Request.Path);
            return Task.CompletedTask;
        }
    }
}
