using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WorkerDemo.Generic.WalzWorkflow;

public static class DependencyInjection
{
    public static IServiceCollection UseSuperWorkflows(
        this IServiceCollection services)
    {
        var step_types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(typeof(WalzStepBodyAsync).IsAssignableFrom)
            .ToArray();


        services.AddTransient<Assets.Factory>(svc => wfid =>
        {
            var wc = svc.GetService<WalzWorkflowConfig>()!;
            var cm = svc.GetService<ClientManager>()!;
            return new Assets(wc, wfid, cm);
        });

        foreach (var t in step_types)
        {
            services.AddTransient(t, svc =>
            {
                var step = (WalzStepBodyAsync)Activator.CreateInstance(t)!;
                step.AssetsFactory = svc.GetService<Assets.Factory>()!;
                return step;
            });
        }
        return services;
    }
}
