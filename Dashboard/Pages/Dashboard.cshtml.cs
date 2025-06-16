using Dashboard.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dashboard.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Dashboard.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ProjectService _projectService;

        public DashboardModel(ApplicationDbContext db, ProjectService projectService)
        {
            _db = db;
            _projectService = projectService;
        }

        public List<Project> Projects { get; set; }

        [BindProperty]
        public Project NewProject { get; set; }

        [BindProperty]
        public Project EditableProject { get; set; }

        public string ErrorMessage { get; set; } 

        public int AllCount { get; set; }
        public int StartedCount { get; set; }
        public int CompletedCount { get; set; }

        public string? UserFullName { get; set; }

        public async Task<IActionResult> OnGet()
        {

            var uid = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(uid))
            {
                return RedirectToPage("/Login");
            }

            int id = int.Parse(uid);

            UserFullName = HttpContext.Session.GetString("UserFullName");

            Projects = await _projectService.GetProjectsForUserAsync(id);

            AllCount = Projects.Count;
            StartedCount = Projects.Count(p => p.Status == ProjectStatus.Started);
            CompletedCount = Projects.Count(p => p.Status == ProjectStatus.Completed);

            return Page();
        }


        public async Task<IActionResult> OnPostAddAsync()
        {
            var uid = HttpContext.Session.GetString("UserId");

            int id = int.Parse(uid);

            NewProject.UserId = id;
            NewProject.Status = ProjectStatus.Started;

            await _projectService.CreateProjectAsync(NewProject);

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var success = await _projectService.DeleteProjectAsync(id);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete project.");
            }

            return RedirectToPage(); 
        }



        public async Task<JsonResult> OnGetProjectAsync(string id)
        {

            int pId = int.Parse(id);

            var project = await _projectService.GetProjectByIdAsync(pId); 
            if (project == null) return new JsonResult(NotFound());
            return new JsonResult(project);
        }


        public async Task<IActionResult> OnPostEditAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToPage("/Login");

            var userId = int.Parse(userIdStr);

            EditableProject.UserId = userId;       

            var success = await _projectService.UpdateProjectAsync(EditableProject);
            if (!success)
            {
                TempData["Error"] = "Failed to update project.";
                return Page();
            }

            TempData["Success"] = "Project updated successfully.";
            return RedirectToPage(); 
        }


        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear(); 

            return RedirectToPage("/Login"); 
        }


    }




}
