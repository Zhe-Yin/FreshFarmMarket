using FarmFreshMarket_201457F.Models;
using FarmFreshMarket_201457F.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FarmFreshMarket_201457F.Pages
{
    [Authorize(Roles = "Admin")]
    public class LogsModel : PageModel
    {

        private readonly LogService _logServce;

        public LogsModel(LogService logService)
        {
            _logServce = logService;
        }

        public List<Log> Loglist { get; set; } = new();
        
        public async Task<IActionResult> OnGetAsync()
        {
            Loglist = await _logServce.RetreiveAllLogs();

            return Page();
        }
    }
}
