using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        public ProjectDbContext _context;
        private readonly IMapper _mapper;
        public EventsController(ProjectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvent()
        {
            var _event = _mapper.Map<List<EventDTO>>(_context.Events.OrderBy(p => p.EventId).ToList());
            return Ok(_event);
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(EventDTO eventDto)
        {
            eventDto.EventId = null;
            var _event = _mapper.Map<Event>(eventDto);
            _context.Events.Add(_event);
            await _context.SaveChangesAsync();

            return Ok("Successful");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Event>> Put(int id, EventDTO eventDto)
        {
            var _event = _mapper.Map<Event>(eventDto);
            _event.EventId = id;
            _context.Events.Update(_event);
            await _context.SaveChangesAsync();
            return Ok("Succeessful");
        }
    }
}
