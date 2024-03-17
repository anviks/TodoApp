using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;
        private readonly Repository _repo;

        public IndexModel(DAL.AppDbContext context, Repository repo)
        {
            _context = context;
            _repo = repo;
        }

        public IList<TodoTask> TodoTask { get; set; } = default!;

        [BindProperty]
        public Guid TaskId { get; set; }

        [BindProperty]
        public bool IsCompleted { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Title Filter")]
        public string? TitleFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Category Filter")]
        public string? CategoryFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Description Filter")]
        public string? DescriptionFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Priority Filter")]
        public EPriority? PriorityFilter { get; set; } = null;

        [BindProperty(SupportsGet = true)]
        public string? SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? SortAscending { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TodoTask = await _context.Tasks
                .Include(t => t.Category)
                .OrderByDescending(task => task.Priority)
                .ThenBy(task => task.Deadline ?? DateTime.MaxValue)
                .ToListAsync();

            // Q: Kuidas tühjasid parameetreid eemaldada, kuidas duplikatsiooni vähendada?

            if (!string.IsNullOrEmpty(TitleFilter))
            {
                TodoTask = TodoTask.Where(task => task.Title
                        .ToLower()
                        .Contains(TitleFilter.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(CategoryFilter))
            {
                TodoTask = TodoTask.Where(task => task.Category?.Name
                        .ToLower()
                        .Contains(CategoryFilter.ToLower()) ?? false)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(DescriptionFilter))
            {
                TodoTask = TodoTask.Where(task => task.Description?
                        .ToLower()
                        .Contains(DescriptionFilter.ToLower()) ?? false)
                    .ToList();
            }

            if (PriorityFilter != null)
            {
                TodoTask = TodoTask.Where(task => task.Priority == PriorityFilter)
                    .ToList();
            }

            TodoTask = SortBy switch
            {
                "Title" => SortAscending == true
                    ? TodoTask.OrderBy(task => task.Title).ToList()
                    : TodoTask.OrderByDescending(task => task.Title).ToList(),
                "Category" => SortAscending == true
                    ? TodoTask.OrderBy(task => task.Category?.Name).ToList()
                    : TodoTask.OrderByDescending(task => task.Category?.Name).ToList(),
                "Description" => SortAscending == true
                    ? TodoTask.OrderBy(task => task.Description).ToList()
                    : TodoTask.OrderByDescending(task => task.Description).ToList(),
                "Priority" => SortAscending == true
                    ? TodoTask.OrderBy(task => task.Priority).ToList()
                    : TodoTask.OrderByDescending(task => task.Priority).ToList(),
                "CreatedAt" => SortAscending == true
                    ? TodoTask.OrderBy(task => task.CreatedAt).ToList()
                    : TodoTask.OrderByDescending(task => task.CreatedAt).ToList(),
                "Deadline" => SortAscending == true
                    ? TodoTask.OrderBy(task => task.Deadline ?? DateTime.MaxValue).ToList()
                    : TodoTask.OrderByDescending(task => task.Deadline ?? DateTime.MaxValue).ToList(),
                _ => TodoTask
            };

            TodoTask = TodoTask.OrderBy(task => task.IsCompleted).ToList();

            return Page();
        }

        public IActionResult OnPostToggleComplete()
        {
            var todoTask = _repo.GetTask(TaskId)!;
            todoTask.IsCompleted = IsCompleted;
            _repo.AddTask(todoTask);

            return RedirectToPage("/Index", new {TitleFilter, CategoryFilter, DescriptionFilter, PriorityFilter, SortBy, SortAscending});
        }
    }
}