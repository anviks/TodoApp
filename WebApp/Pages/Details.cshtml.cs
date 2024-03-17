using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public TodoTask TodoTask { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todotask = await _context.Tasks
                .Include(task => task.Category)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (todotask == null)
            {
                return NotFound();
            }
            else
            {
                TodoTask = todotask;
            }
            return Page();
        }
    }
}
