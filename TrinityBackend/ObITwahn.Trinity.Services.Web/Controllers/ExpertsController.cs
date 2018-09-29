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
    public class ExpertsController : ControllerBase
    {
        private readonly Repository<Employee> _employeeRepository;
        private TrinityContext _context;
        private Repository<Meeting> _meetingRepositry;

        public ExpertsController(Repository<Employee> employeeRepository, Repository<Meeting> meetingRepository, TrinityContext context)
        {
            _employeeRepository = employeeRepository;
            _meetingRepositry = meetingRepository;
            _context = context;
        }

        //// GET: api/Experts
        //[HttpGet]
        //public async Task<IEnumerable<Employee>> GetEmloyees()
        //{
        //    return await _employeeRepository.GetAllAsync();
        //}

        // GET: api/Experts/Topic
        [HttpGet]
        public async Task<IEnumerable<Employee>> GetEmloyees([FromQuery] string topic)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));

            var meetingsWithTopic = await _meetingRepositry.GetAllAsync(m =>
                (!String.IsNullOrWhiteSpace(m.Subject) &&
                 m.Subject.Contains(topic, StringComparison.InvariantCultureIgnoreCase)) ||
                (!String.IsNullOrWhiteSpace(m.Comment) &&
                 m.Comment.Contains(topic, StringComparison.InvariantCultureIgnoreCase)) ||
                (!String.IsNullOrWhiteSpace(m.Minutes) &&
                 m.Minutes.Contains(topic, StringComparison.InvariantCultureIgnoreCase)));

            var employeeWithTopic = await _employeeRepository.GetAllAsync(m =>
                (!String.IsNullOrWhiteSpace(m.Title) &&
                 m.Title.Contains(topic, StringComparison.InvariantCultureIgnoreCase))
                || (!String.IsNullOrWhiteSpace(m.Department) &&
                    m.Department.Contains(topic, StringComparison.InvariantCultureIgnoreCase)));

            var employeesWithPointFromMeetings = meetingsWithTopic.Select(m => new
                {
                    Meeting = m,
                    SubjectPoints = m.Subject.CountOccurrencies(topic) * 2,
                    CommentPoints = m.Comment.CountOccurrencies(topic) * 1,
                    MinutesPoints = m.Minutes.CountOccurrencies(topic) * 10
                })
                .Select(x => new
                {
                    Meeting = x.Meeting,
                    Sum = x.SubjectPoints + x.CommentPoints + x.MinutesPoints
                })
                .SelectMany(m =>
                    m.Meeting.Participants.Select(p => new
                        {EmployeeId = p.Employee.Id, Employee = p.Employee, Points = m.Sum}));

            var groups = employeesWithPointFromMeetings
                .Union(employeeWithTopic.Select(x => new {EmployeeId = x.Id, Employee = x, Points = 1}))
                .GroupBy(x => x.EmployeeId);

            var resultList = groups.Select(g => new {Employee = g.First().Employee, Points = g.Sum(x => x.Points)})
                .OrderByDescending(x => x.Points).ToList();



            return resultList.Take(3).Select(x => x.Employee).ToList(); // Top 3 items

        
        }

        // GET: api/Experts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeRepository.GetAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Experts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] Guid? id, [FromBody] ObITwahn.Services.Meeting.Model.Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            await _employeeRepository.SaveOrUpdateAsync(employee);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        // POST: api/Experts
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] ObITwahn.Services.Meeting.Model.Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _employeeRepository.SaveOrUpdateAsync(employee);

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Experts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _employeeRepository.DeleteAsync(id);

            return Ok();
        }

    
    }
}