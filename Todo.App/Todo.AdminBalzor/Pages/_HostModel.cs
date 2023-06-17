using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Todo.AdminBlazor.Pages;

public class _HostModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
    

        return Page();
    }
}