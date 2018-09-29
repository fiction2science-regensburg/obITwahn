using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using obITwahn.Services.Common;

namespace ObITwahn.Services.Meeting.Model
{
    public class Employee : EntityBase
    {
        [Required]
        public string Name { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        [JsonIgnore]
        public ICollection<EmployeeMeeting> Meetings { get; } = new List<EmployeeMeeting>();
    }
}
