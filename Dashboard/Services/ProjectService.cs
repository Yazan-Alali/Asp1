using System;
using Dashboard.Models;
using Microsoft.EntityFrameworkCore;



namespace Dashboard.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _db;

        public ProjectService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Project>> GetProjectsForUserAsync(int userId)
        {
            return await _db.Projects
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.StartDate)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _db.Projects
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return project;
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId);
            if (project == null) return false;

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProjectAsync(Project updatedProject)
        {
            var existing = await _db.Projects
                .FirstOrDefaultAsync(p => p.Id == updatedProject.Id && p.UserId == updatedProject.UserId);

            if (existing == null) return false;

            existing.Title = updatedProject.Title;
            existing.Client = updatedProject.Client;
            existing.Description = updatedProject.Description;
            existing.StartDate = updatedProject.StartDate;
            existing.EndDate = updatedProject.EndDate;
            existing.Budget = updatedProject.Budget;
            existing.Status = updatedProject.Status;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
