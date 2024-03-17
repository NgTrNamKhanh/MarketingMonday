using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Comp1640_Final.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        public ProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventService _eventService;
        public EventsController(ProjectDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IEventService eventService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var _event = _mapper.Map<List<EventDTO>>(_eventService.GetEvents());
            return Ok(_event);
        }

        [HttpGet("student/{userId}")]
        public async Task<ActionResult<EventDTO>> GetStudentEvent(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest("There is no user");
                }
                else
                {
                    var facultyId = user.FacultyId;
                    var singleEvent = await _eventService.GetFirstEventByFaculty(facultyId);
                    if (singleEvent == null)
                    {
                        return NotFound("No event found for the user's faculty");
                    }

                    var eventResult = _mapper.Map<EventDTO>(singleEvent);
                    return Ok(eventResult);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(EventDTO eventDto)
        {
            eventDto.Id = null;
            var _event = _mapper.Map<Event>(eventDto);
            var addResult = await _eventService.CreateEvent(_event);
            if (!addResult)
            {
                return BadRequest("Error when creating event");
            }
            else 
            {
                return Ok("Successful");

            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Event>> Put(int id, EventDTO eventDto)
        {
            var _event = _mapper.Map<Event>(eventDto);
            _event.Id = id;
            var editResult = await _eventService.EditEvent(_event);
            if (!editResult)
            {
                return BadRequest("Error when editing event");
            }
            else
            {
                return Ok("Successful");

            }
        }
    }
}
