using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sample.DistributedTracing
{
    class Program
    {
        private static ActivitySource source = new ActivitySource(nameof(Sample.DistributedTracing));

        static async Task Main(string[] args)
        {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyService"))
                .AddSource(nameof(Sample.DistributedTracing))
                .AddConsoleExporter()
                .Build();

            await Execute();            
        }
        
        static async Task Execute()
        {
            using (var activity = source.StartActivity(nameof(Execute)))
            {
                await StepOne();                
                await StepTwo();                
            }
        }

        static async Task StepOne()
        {
            using (var activity = source.StartActivity("StepOne"))
            {
                activity?.SetTag("tag1", "value1");
                activity?.SetTag("tag2", "value2");
                await Task.Delay(500);
            }
        }

        static async Task StepTwo()
        {
            using (var activity = source.StartActivity("StepTwo"))
            {
                activity?.AddEvent(new ActivityEvent("StepTwo Event"));
                await Task.Delay(1000);
            }
        }
    }
}