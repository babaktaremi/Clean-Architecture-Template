using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse:class
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            

            try
            {
                var response = await next();
                return response;
            }
            catch (Exception e)
            {
               _logger.LogError(e,e.Message);

               if (typeof(TResponse).GetGenericTypeDefinition() == typeof(OperationResult<>))
               {
                   var response=new OperationResult<TResponse>{IsException = true};
                   
                   return response as TResponse; 
               }

               return default;
            }

          
        }
    }
}
