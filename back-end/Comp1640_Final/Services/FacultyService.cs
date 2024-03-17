using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{
    public interface IFacultyService
    {
        ICollection<Faculty> GetFaculties();
    }

    public class FacultyService : IFacultyService
    {
        public readonly ProjectDbContext _context;

        public FacultyService(ProjectDbContext context)
        {
            _context = context;
        }

        public ICollection<Faculty> GetFaculties()
        {
            return _context.Faculties.OrderBy(p => p.Id).ToList();
        }
    }
}
