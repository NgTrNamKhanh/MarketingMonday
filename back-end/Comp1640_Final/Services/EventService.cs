using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{
    public interface IEventService
    {
        ICollection<Event> GetEvents();
        Event GetEventByID(int id);
        Task<Event> GetFirstEventByFaculty(int facultyId);
        Task<bool> CreateEvent(Event ev);

        Task<bool> EditEvent(Event ev);
        Task<bool> Save();

    }
    public class EventService : IEventService
    {
        private readonly ProjectDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EventService(ProjectDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<Event> GetFirstEventByFaculty(int facultyid)
        {
            return await _context.Events.FirstOrDefaultAsync(p => p.FacultyId == facultyid);
        }

        public Event GetEventByID(int id)
        {
            return _context.Events.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Event> GetEvents()
        {
            return _context.Events.OrderBy(p => p.Id).ToList();
        }

        public async Task<bool> CreateEvent(Event ev)
        {
            _context.Events.Add(ev);
            return await Save();
        }
        public async Task<bool> EditEvent(Event ev)
        {
            _context.Events.Update(ev);
            return await Save();
        }

    }
}
