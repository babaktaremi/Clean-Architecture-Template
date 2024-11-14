using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.MSSqlServer;

namespace CleanArc.Infrastructure.CrossCutting.Logging;

public static class LoggingConfiguration
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger => (context, configuration) =>
    {
        #region Enriching Logger Context

        var env = context.HostingEnvironment;


        configuration.Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", env.ApplicationName)
            .Enrich.WithProperty("Environment", env.EnvironmentName)
            .Enrich.WithSpan()
            .Enrich.WithExceptionDetails();
            


        #endregion


        var columnOpts = new ColumnOptions();
        columnOpts.Store.Remove(StandardColumn.Properties);
        columnOpts.Store.Add(StandardColumn.LogEvent);
        columnOpts.LogEvent.DataLength = 4096;
        columnOpts.PrimaryKey = columnOpts.Id;
        columnOpts.Id.DataType = SqlDbType.Int;

        if (!context.HostingEnvironment.IsDevelopment())
        {
            configuration.WriteTo
                .MSSqlServer(
                    connectionString: context.Configuration.GetConnectionString("logDb"),
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents", AutoCreateSqlTable = true, SchemaName = "log",AutoCreateSqlDatabase = true})
                .MinimumLevel.Warning();

        }

        else{
            configuration.WriteTo.Console().MinimumLevel.Information();
            configuration.WriteTo.File(new JsonFormatter(), "logs/log.json").MinimumLevel.Information();
        }

        #region ElasticSearch Configuration. UnComment if Needed 


        //var elasticUrl = context.Configuration.GetValue<string>("Logging:ElasticUrl");

        //if (!string.IsNullOrEmpty(elasticUrl))
        //{
        //    configuration.WriteTo.Elasticsearch(
        //        new ElasticsearchSinkOptions(new Uri(elasticUrl))
        //        {
        //            AutoRegisterTemplate = true,
        //            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        //            IndexFormat = "web-logs-{0:yyyy.MM.dd}",
        //            MinimumLogEventLevel = LogEventLevel.Debug
        //        });
        //}

        #endregion
    };
}