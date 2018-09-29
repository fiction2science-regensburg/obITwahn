using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ObITwahn.Services.Meeting.Model;
using ObITwahn.Trinity.Services.Web.Data;

namespace ObITwahn.Trinity.Services.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly Repository<Meeting> _repository;

        public MeetingsController(Repository<Meeting> repository)
        {
            _repository = repository;
        }

        // GET: api/Meetings
        [HttpGet]
        public async Task< IEnumerable<Meeting>> GetMeetings()
        {
          return await _repository.GetAllAsync();
        }

        // GET: api/Meetings/28.09.2018?name=hans
        [HttpGet("{date:datetime}")]
        public async Task<IEnumerable<Meeting>> GetMeetings([FromQuery] string names, [FromRoute] DateTime? date)
        {
            if (date == null) throw new ArgumentNullException(nameof(date));

            var namesList = names?.Split(',')
                                 .Select(n => n.ToLower());

            //var meeting = await _repository.GetAllAsync(m => m.Start.HasValue && m.Start.Value.Date == date.Value.Date && namesList.Any(name => m.Participants.Any(p => p.Employee.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))));

            //var meeting = await _repository.GetAllAsync(m => m.Start >= date && m.End <= date);

            var meetings = _repository.GetAllAsync(m => namesList.Any(name => m.Participants.Any(p => p.Employee.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)))).Result;

            return meetings;

          
        }


        // GET: api/Meetings/5
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMeeting([FromRoute] Guid? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meeting = await _repository.GetAsync(id);

            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(meeting);
        }

        // PUT: api/Meetings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeeting([FromRoute] Guid? id, [FromBody] Meeting meeting)
        {
            if (id != meeting.Id)
            {
                return BadRequest();
            }

            await _repository.SaveOrUpdateAsync(meeting);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        // POST: api/Meetings
        [HttpPost]
        public async Task<IActionResult> PostMeeting([FromBody] Meeting meeting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.SaveOrUpdateAsync(meeting);

            return CreatedAtAction("GetMeeting", new { id = meeting.Id }, meeting);
        }

        // DELETE: api/Meetings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting([FromRoute] Guid? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.DeleteAsync(id);
                   
            return Ok();
        }
    }
}