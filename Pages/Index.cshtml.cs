using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Model;
using WorkflowCore.Interface;

namespace WorkerDemo.Pages
{
    public class IndexModel : PageModel
    {
        readonly ILogger<IndexModel> _logger;
        readonly IWorkflowHost wf;

        public Workflow[] Suspended { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IWorkflowHost wf)
        {
            _logger = logger;
            this.wf = wf;
        }

        public async void OnGet()
        {
            using var db = new WorkflowContext();
            Suspended = await db.Workflows
                .ToArrayAsync();
        }

        public async Task<IActionResult> OnPost(string instanceId)
        {
            bool ok = await wf.ResumeWorkflow(instanceId);
            return RedirectToPage();
        }
    }
}
