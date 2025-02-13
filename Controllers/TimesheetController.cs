using EmployeeManagment.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class TimesheetController : ControllerBase
{
    private readonly TimesheetRepository _timesheetRepository;

    // Constructor with dependency injection for TimesheetRepository
    public TimesheetController(TimesheetRepository timesheetRepository)
    {
        _timesheetRepository = timesheetRepository;
    }

    // GET: api/timesheet
    [HttpGet]
    public IActionResult GetAllTimesheets()
    {
        List<Timesheet> timesheets = _timesheetRepository.GetAllTimesheets();
        return Ok(timesheets);  // Return 200 OK with timesheet records
    }

    // POST: api/timesheet/new
    [HttpPost("new")]
    public IActionResult CreateTimesheet([FromBody] Timesheet timesheet)
    {
        if (timesheet == null)
        {
            return BadRequest("Timesheet data is null.");  // Return 400 Bad Request
        }

        try
        {
            _timesheetRepository.CreateTimesheet(timesheet);
            return CreatedAtAction(nameof(GetAllTimesheets), new { employeeId = timesheet.EmployeeId }, timesheet);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");  // Return 500 Internal Server Error
        }
    }
    [HttpGet("Details")]
    public ActionResult<IEnumerable<TimesheetDetails>> GetTimesheetDetails()
    {
        try
        {
            var details = _timesheetRepository.GetTimesheetDetails();
            return Ok(details);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }// GET: api/timesheet/{id}
    [HttpGet("{id}")]
    public IActionResult GetTimesheetById(int id)
    {
        var timesheet = _timesheetRepository.GetTimesheetById(id);

        if (timesheet == null)
        {
            return NotFound($"Timesheet with ID {id} not found.");
        }

        return Ok(timesheet);
    }
    [HttpPut("update/{timesheetId}")]
    public IActionResult UpdateTimesheet(int timesheetId, [FromBody] Timesheet timesheet)
    {
        if (timesheet == null)
        {
            return BadRequest("Invalid timesheet data.");
        }

        try
        {
            bool isUpdated = _timesheetRepository.UpdateTimesheet(timesheetId, timesheet);

            if (isUpdated)
                return Ok($"Timesheet with ID {timesheetId} updated successfully.");
            else
                return NotFound($"Timesheet with ID {timesheetId} not found.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
