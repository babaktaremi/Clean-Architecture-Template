using System.Diagnostics;
using System.Diagnostics.Metrics;
using Mediator;

namespace CleanArc.Application.Common;

public class MetricsBehaviour<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse>  where TRequest : IRequest<TResponse>
{
    private readonly Histogram<long> _requestResponseDurationHistogram;

    public MetricsBehaviour(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("mediator_meter");
        _requestResponseDurationHistogram = meter.CreateHistogram<long>(
            "Request_Response_Duration", "ms"
            , "Determines the total request response durations");
    }
    
    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var stopWatch = Stopwatch.StartNew();

        var response = await next(message,cancellationToken);
        
        stopWatch.Stop();
        
        _requestResponseDurationHistogram.Record(stopWatch.ElapsedMilliseconds,new []{new KeyValuePair<string, object>("Request",message.GetType().Name)});

        return response;
    }
}