namespace EmployeeManagment.Services
{
    public class Timesheet
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string WorkSummary { get; set; }
    }
}