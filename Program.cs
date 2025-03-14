using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Generic.WalzWorkflow;
using Workflow_Demo;
using WorkflowCore.Interface;
using WorkflowCore.Services;

var builder = WebApplication.CreateBuilder(args);

var workflow_cs = builder.Configuration.GetConnectionString("workflow")!;

///////////////////////////////////////////////////////////////////////////////
// Read config for WalzWorkflow
builder.Services.AddSingleton(
    builder.Configuration.GetSection("WalzWorkflow")
        .Get<WalzWorkflowConfig>()!);
// ----------------------------------------------------------------------------

builder.Services
    .AddWorkflow(x => x.UseSqlite(workflow_cs, true))
    .AddHostedService<WorkflowHost>()
    // used that in the past to directly access the workflow api
    // (before I found IWorkflowPurger in the API, which is better of course)
    //.AddDbContext<WorkflowContext>(opt => opt.UseSqlite(workflow_cs))
    .AddWalzWorkflows()
    .AddWorkflowMiddleware<WorkflowStateMiddleware>()
    .AddSingleton<ClientManager>()
    .AddRazorPages();

builder.Services.AddSignalR();

///////////////////////////////////////////////////////////////////////////////
// Authentication Configuration
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
    {
        //opt.Cookie.HttpOnly = true;
        opt.Cookie.Name = "WorkerDemo";
    });
// ----------------------------------------------------------------------------

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
// Authentication Services
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();
// ----------------------------------------------------------------------------

app.MapHub<WalzWorkflowHub>("/walzworkflowhub");

///////////////////////////////////////////////////////////////////////////////
// Application specific stuff
var wf = app.Services.GetService<IWorkflowHost>()!;
wf.RegisterWorkflow<DemoWorkflow>();
// ----------------------------------------------------------------------------

///////////////////////////////////////////////////////////////////////////////
// make sure db directory exists
var dir = Path.GetDirectoryName(workflow_cs.Split("=", 2)[1])!;
if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
// ----------------------------------------------------------------------------

app.Run();
