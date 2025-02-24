using Microsoft.EntityFrameworkCore;
using WorkerDemo.Model;
using WorkflowCore.Interface;
using WorkflowCore.Services;

var builder = WebApplication.CreateBuilder(args);

var workflow_cs = builder.Configuration.GetConnectionString("workflow")!;

builder.Services
    .AddWorkflow(x => x.UseSqlite(workflow_cs, true))
    .AddHostedService<WorkflowHost>()
    .AddDbContext<WorkflowContext>(opt => opt.UseSqlite(workflow_cs))
    .AddRazorPages();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


///////////////////////////////////////////////////////////////////////////////
var wf = app.Services.GetService<IWorkflowHost>()!;
wf.RegisterWorkflow<DemoWorkflow>();
///////////////////////////////////////////////////////////////////////////////
var dir = Path.GetDirectoryName(workflow_cs.Split("=", 2)[1])!;
if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
///////////////////////////////////////////////////////////////////////////////


app.MapGet("/demo", async ctx =>
{
    await wf.StartWorkflow("Demo");
    ctx.Response.Redirect("/");
});


app.Run();
