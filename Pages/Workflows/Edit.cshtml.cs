using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkerDemo.Pages.Workflows
{
    public class EditModel : PageModel
    {
        public WorkflowInstance? Workflow { get; set; }
        public string Title = "";
        readonly IWorkflowHost workflows;

        public EditModel(IWorkflowHost workflows)
        {
            this.workflows = workflows;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Workflow = await workflows.PersistenceStore.GetWorkflowInstance(id);
            if (Workflow == null) return NotFound();
            Title = $"{Workflow.WorkflowDefinitionId}/{Workflow.Reference}";
            return Page();
        }
    }
}
