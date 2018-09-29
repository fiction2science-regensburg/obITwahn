using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using obITwahn.Services.Common;

namespace ObITwahn.Services.Meeting.Model
{
    public class Meeting : EntityBase
    {
        [Required]
        public string Subject { get; set; }

        [JsonIgnore]
        public ICollection<EmployeeMeeting> Participants { get; set; } = new List<EmployeeMeeting>();

        public string Comment { get; set; }

        public string Minutes { get; set; }

        [Required]
        public DateTime? Start { get; set; }

        [Required]
        public DateTime? End { get; set; }
    }
}