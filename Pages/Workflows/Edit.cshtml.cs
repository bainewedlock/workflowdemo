using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkerDemo.Generic.WalzWorkflow;
using WorkerDemo.Generic.WorkflowEF;

namespace WorkerDemo.Pages.Workflows
{
    public class EditModel : PageModel
    {
        public Workflow Workflow { get; set; } = default!;
        public List<LogEntry> LogEntries { get; set; } = [];
        readonly WorkflowContext db;
        readonly Assets.Factory assets;

        public EditModel(WorkflowContext db, Assets.Factory assets)
        {
            this.db = db;
            this.assets = assets;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || await db.Workflows.FirstOrDefaultAsync(
                m => m.InstanceId == new Guid(id)) == null) return NotFound();

            Workflow = await db.Workflows.SingleAsync(
                m => m.InstanceId == new Guid(id));

            var a = assets(id);

            foreach (var l in await a.ReadStringLinesAsync("logfile.txt"))
            {
                var tokens = l.Split();
                var left = string.Join(" ", tokens.Take(2));
                var right = string.Join(" ", tokens.Skip(2));

                LogEntries.Add(new LogEntry
                {
                    Time = left,
                    Message = right
                });
            }

            LogEntries.Sort((a, b) => a.CompareTo(b));
            return Page();
        }
    }
}
