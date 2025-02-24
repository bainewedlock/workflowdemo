using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Model;
using WorkflowCore.Interface;

namespace WorkerDemo.Pages.Workflows
{
    public class EditModel : PageModel
    {
        readonly WorkflowContext db;
        readonly IWorkflowHost wf;

        public EditModel(WorkflowContext db, 
            IWorkflowHost wf)
        {
            this.db = db;
            this.wf = wf;
        }

        public Workflow Workflow { get; set; } = default!;
        //public Workflow Workflow { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflow = await db.Workflows.FirstOrDefaultAsync(m => m.PersistenceId == id);
            if (workflow == null)
            {
                return NotFound();
            }
            else
            {
                Workflow = workflow;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostResumeAsync(string persistenceId, string instanceId)
        {
            bool ok = await wf.ResumeWorkflow(instanceId);
            return RedirectToPage("./Edit", new { id = persistenceId });
        }
    }
}
