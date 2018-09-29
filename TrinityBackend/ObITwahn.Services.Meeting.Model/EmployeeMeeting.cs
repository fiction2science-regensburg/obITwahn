using obITwahn.Services.Common;

namespace ObITwahn.Services.Meeting.Model
{
    public class EmployeeMeeting : EntityBase
    {
        public Employee Employee { get; set; }
        public Meeting Meeting { get; set; }
    }
}