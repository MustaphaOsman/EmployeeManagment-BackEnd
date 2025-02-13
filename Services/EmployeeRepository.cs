using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class EmployeeRepository
{
    private readonly string _connectionString;

    // Constructor to initialize the connection string
    public EmployeeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Method to get all employees from the database
    public List<Employee> GetAllEmployees()
    {
        List<Employee> employees = new List<Employee>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();  // Open the database connection

            using (var command = new SqlCommand("GetAllEmployees", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var employee = new Employee
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Email = (string)reader["Email"],
                       
                            JobTitle = (string)reader["JobTitle"],
                       
                            Salary = (decimal)reader["Salary"],
                            StartDate = (DateTime)reader["StartDate"],
                         
                        };
                        employees.Add(employee);
                    }
                }
            }
        }

        return employees;
    }


    public void CreateEmployee(Employee employee)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();  // Open the database connection

            using (var command = new SqlCommand("CreateEmployee", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters to the command
                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                command.Parameters.AddWithValue("@DOB", employee.DOB);
                command.Parameters.AddWithValue("@JobTitle", employee.JobTitle);
                command.Parameters.AddWithValue("@Department", employee.Department);
                command.Parameters.AddWithValue("@Salary", employee.Salary);
                command.Parameters.AddWithValue("@StartDate", employee.StartDate);

                // Execute the stored procedure
                command.ExecuteNonQuery();
            }
        }
    }


    public void UpdateEmployee(Employee employee)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();  // Open the database connection

                using (var command = new SqlCommand("UpdateEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    command.Parameters.AddWithValue("@EmployeeID", employee.Id);
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                    command.Parameters.AddWithValue("@DOB", employee.DOB);
                    command.Parameters.AddWithValue("@JobTitle", employee.JobTitle);
                    command.Parameters.AddWithValue("@Department", employee.Department);
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@StartDate", employee.StartDate);

                    // Handle nullable EndDate
                    if (employee.EndDate.HasValue)
                    {
                        command.Parameters.AddWithValue("@EndDate", employee.EndDate.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@EndDate", DBNull.Value);
                    }

                    // Execute the stored procedure
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception (you can use your own logging mechanism)
            Console.WriteLine("Error updating employee: " + ex.Message);
            // Optionally, rethrow the exception or handle it based on your use case
        }
    }



    // Method to get a single employee by ID from the database
    public Employee GetEmployeeById(int id)
    {
        Employee employee = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();  // Open the database connection

            using (var command = new SqlCommand("GetEmployeeById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add the @Id parameter to the command
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())  // If we find a matching record
                    {
                        employee = new Employee
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Email = (string)reader["Email"],
                            PhoneNumber = (string)reader["PhoneNumber"],
                            DOB = (DateTime)reader["DOB"],
                            JobTitle = (string)reader["JobTitle"],
                            Department = (string)reader["Department"],
                            Salary = (decimal)reader["Salary"],
                            StartDate = (DateTime)reader["StartDate"],
                            EndDate = reader["EndDate"] as DateTime?  // Nullable field
                        };
                    }
                }
            }
        }

        return employee;  // If no match, it will return null
    }

}
