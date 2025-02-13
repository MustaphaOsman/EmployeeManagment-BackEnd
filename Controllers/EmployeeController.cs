using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]  // This attribute ensures automatic model validation and response formatting
[Route("api/[controller]")]  // Route that maps to /api/employee
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _employeeRepository;

    // Constructor with dependency injection for EmployeeRepository
    public EmployeeController(EmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    // GET: api/employee
    [HttpGet]  // Maps to the GET request for fetching all employees
    public IActionResult GetAllEmployees()
    {
        List<Employee> employees = _employeeRepository.GetAllEmployees();
        return Ok(employees);  // Return the list of employees with a 200 OK response
    }

    // GET: api/employee/{id}
    [HttpGet("{id}")]  // Maps to the GET request for fetching a specific employee by ID
    public IActionResult GetEmployeeById(int id)
    {
        var employee = _employeeRepository.GetEmployeeById(id);

        if (employee == null)
        {
            return NotFound();  // Return 404 if the employee was not found
        }

        return Ok(employee);  // Return the found employee with a 200 OK response
    }
    // POST: api/employee
    [HttpPost("new")]  // Maps to the POST request for creating an employee
    public IActionResult CreateEmployee([FromBody] Employee employee)
    {
        if (employee == null)
        {
            return BadRequest("Employee data is null.");  // Return a 400 Bad Request if the employee data is null
        }

        try
        {
            _employeeRepository.CreateEmployee(employee);  // Call the repository method to create the employee
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);  // Return 201 Created with the location of the new resource
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");  // Return 500 if an error occurs
        }
    }

    // PUT: api/employee/{id}
    [HttpPut("{id}")]  // Maps to the PUT request for updating an employee by ID
    public IActionResult UpdateEmployee(int id, [FromBody] Employee employee)
    {
        if (employee == null)
        {
            return BadRequest("Employee data is null.");  // Return a 400 Bad Request if the employee data is null
        }

        if (id != employee.Id)
        {
            return BadRequest("Employee ID mismatch.");  // Return a 400 Bad Request if the ID doesn't match
        }

        var existingEmployee = _employeeRepository.GetEmployeeById(id);
        if (existingEmployee == null)
        {
            return NotFound();  // Return 404 if the employee doesn't exist
        }

        try
        {
            _employeeRepository.UpdateEmployee(employee);  // Call the repository method to update the employee
            return NoContent();  // Return 204 No Content if the update is successful (no body in response)
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");  // Return 500 if an error occurs
        }
    }

}
