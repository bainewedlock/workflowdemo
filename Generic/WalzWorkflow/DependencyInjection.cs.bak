using System.Reflection;

namespace WorkerDemo.Generic.WalzWorkflow;

public static class DependencyInjection
{
    public static IServiceCollection AddWalzWorkflows(
        this IServiceCollection services)
    {
        var step_types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(typeof(WalzStepBodyAsync).IsAssignableFrom)
            .ToArray();

        services.AddWorkflowMiddleware<WorkflowStateMiddleware>();
        services.AddSingleton<ClientManager>();
        services.AddHostedService<CleanupWorkflowService>();

        services.AddTransient<Assets.Factory>(svc => wf =>
        {
            var wc = svc.GetService<WalzWorkflowConfig>()!;
            var cm = svc.GetService<ClientManager>()!;
            return new Assets(wc, wf, cm);
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
