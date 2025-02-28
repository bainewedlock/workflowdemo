using System.Reflection;

namespace WorkerDemo.Generic.Workflows;

public static class DependencyInjection
{
    public static IServiceCollection UseWorkitemSteps(
        this IServiceCollection services)
    {
        var step_types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(typeof(WorkItemStepAsync).IsAssignableFrom)
            .ToArray();

        foreach (var t in step_types)
        {
            services.AddTransient(t, svc =>
            {
                var step = (WorkItemStepAsync)Activator.CreateInstance(t)!;
                step.WorkflowConfig = svc.GetService<WorkflowConfig>()!;
                step.ClientManager = svc.GetService<ClientManager>()!;
                return step;
            });
        }
        return services;
    }
}
