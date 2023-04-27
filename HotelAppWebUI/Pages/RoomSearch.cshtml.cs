using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelAppWebUI.Pages
{
    public class RoomSearchModel : PageModel
    {
        [DataType(DataType.Date)]
        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
