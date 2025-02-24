using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorkerDemo.Pages
{
    public class IndexModel : PageModel
    {
        readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async void OnGet()
        {
        }
    }
}
