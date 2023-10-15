using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Wordly.Core.Options;

namespace Wordly.Api.Extensions;

internal static class HostExtensions
{
    public static IHostBuilder AddLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        var loggingOptions = configuration.GetSection("Logging").Get<LoggingOptions>();

        hostBuilder.UseSerilog((_, logger) =>
        {
            logger
                .Enrich.FromLogContext()
                .WriteTo.Console(loggingOptions.ConsoleLogLevel);
        });

        hostBuilder.ConfigureWebHost(builder => builder.Configure(app => app.UseSerilogRequestLogging()));

        return hostBuilder;
    }
}