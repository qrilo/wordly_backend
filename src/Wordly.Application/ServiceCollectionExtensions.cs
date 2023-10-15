using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Wordly.Application.Consumers;
using Wordly.Application.Contracts;
using Wordly.Application.Jobs;
using Wordly.Application.Mapping;
using Wordly.Application.Services;
using Wordly.Application.Validators.Auth;
using Wordly.Application.Validators.Terms;
using Wordly.Core.Common;
using Wordly.Core.Contracts;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Wordly.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IObjectsMapper, ObjectsMapper>();
        services.AddScoped<IHashingProvider, HashingProvider>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ITermsService, TermsService>();
        services.AddScoped<IBlobService, BlobService>();

        services.AddScoped<IAuthValidatorsAggregate, AuthValidatorsAggregate>();
        services.AddScoped<ITermValidatorsAggregate, TermValidatorsAggregate>();

        return services;
    }

    public static IServiceCollection AddBroker(this IServiceCollection services)
    {
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer(typeof(SampleCommandConsumer));

            configurator.UsingInMemory((context, config) => { config.ConfigureEndpoints(context); });
        });

        return services;
    }

    public static IServiceCollection AddJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(quartz =>
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Information)
                .AddConsole());

            quartz.SchedulerId = "Scheduler-Main";

            quartz.UseMicrosoftDependencyInjectionJobFactory();
            quartz.UseSimpleTypeLoader();
            quartz.UseInMemoryStore();
            quartz.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 5; });

            quartz.ScheduleJob<SampleJob>(trigger =>
                {
                    var cronExpression = configuration["Jobs:SampleJobCronExpression"]!;

                    trigger.WithIdentity(nameof(SampleJob))
                        .StartNow()
                        .WithCronSchedule(CronScheduleBuilder.CronSchedule(cronExpression));
                }
            );
        });

        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

        return services;
    }
}