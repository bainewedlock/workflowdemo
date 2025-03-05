using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Generic.WalzWorkflow;
using WorkerDemo.Generic.WorkflowEF;
using Workflow_Demo;
using WorkflowCore.Interface;
using WorkflowCore.Services;

var builder = WebApplication.CreateBuilder(args);

var workflow_cs = builder.Configuration.GetConnectionString("workflow")!;

builder.Services.AddSingleton(
    builder.Configuration.GetSection("WalzWorkflow")
        .Get<WalzWorkflowConfig>()!);

builder.Services
    .AddWorkflow(x => x.UseSqlite(workflow_cs, true))
    .AddHostedService<WorkflowHost>()
    .AddDbContext<WorkflowContext>(opt => opt.UseSqlite(workflow_cs))
    .AddWalzWorkflows()
    .AddWorkflowMiddleware<WorkflowStateMiddleware>()
    .AddSingleton<ClientManager>()
    .AddRazorPages();


builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(c => { c.Cookie.Name = "WorkerDemo"; });

builder.Services.AddSignalR();

///////////////////////////////////////////////////////////////////////////////

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

///////////////////////////////////////////////////////////////////////////////
// BEGIN Authentication Services
///////////////////////////////////////////////////////////////////////////////
app.UseAuthentication();
app.UseAuthorization();
///////////////////////////////////////////////////////////////////////////////
// END Authentication Services
///////////////////////////////////////////////////////////////////////////////

app.MapRazorPages();
app.MapHub<WalzWorkflowHub>("/walzworkflowhub");

///////////////////////////////////////////////////////////////////////////////
// Application specific stuff
///////////////////////////////////////////////////////////////////////////////
var wf = app.Services.GetService<IWorkflowHost>()!;
wf.RegisterWorkflow<DemoWorkflow>();
///////////////////////////////////////////////////////////////////////////////
// make sure db directory exists
var dir = Path.GetDirectoryName(workflow_cs.Split("=", 2)[1])!;
if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
///////////////////////////////////////////////////////////////////////////////

app.MapGet("/demo", async ctx =>
{
    //var rnd = new Random().Next(10000000, 90000000);
    var rnd = 12345;
    await wf.StartWorkflow("Demo", null, $"61.{rnd}.1.2");
    ctx.Response.Redirect("/Workflows");
});


app.Run();
