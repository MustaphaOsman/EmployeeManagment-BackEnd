using EmployeeManagment.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class TimesheetRepository
{
    private readonly string _connectionString;

    // Constructor to initialize the connection string
    public TimesheetRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Method to get all timesheet records from the database
    public List<Timesheet> GetAllTimesheets()
    {
        List<Timesheet> timesheets = new List<Timesheet>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();  // Open the database connection

            using (var command = new SqlCommand("GetAllTimesheets", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var timesheet = new Timesheet
                        {
                            Id = (int)reader["id"],
                            EmployeeName = reader["employee_name"].ToString(), // Use EmployeeName instead of EmployeeId
                            StartTime = (DateTime)reader["start_time"],
                            EndTime = (DateTime)reader["end_time"],
                            WorkSummary = reader["work_summary"].ToString()
                        };
                        timesheets.Add(timesheet);

                    }
                }
            }
        }

        return timesheets;
    }

    // Method to create a new timesheet entry
    public void CreateTimesheet(Timesheet timesheet)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();  // Open the database connection

            using (var command = new SqlCommand("CreateTimesheet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters to the command
                command.Parameters.AddWithValue("@EmployeeId", timesheet.EmployeeId);
                command.Parameters.AddWithValue("@StartTime", timesheet.StartTime);
                command.Parameters.AddWithValue("@EndTime", timesheet.EndTime);
                command.Parameters.AddWithValue("@WorkSummary", timesheet.WorkSummary);

                // Execute the stored procedure
                command.ExecuteNonQuery();
            }
        }
    }
    public List<TimesheetDetails> GetTimesheetDetails()
    {
        List<TimesheetDetails> timesheetDetails = new List<TimesheetDetails>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();  // Open the database connection

            using (var command = new SqlCommand("GetTimesheetDetails", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var details = new TimesheetDetails
                        {
                            Id = reader["id"].ToString(),
                            Title = reader["title"].ToString(),
                            Start = ((DateTime)reader["start"]).ToString("yyyy-MM-dd HH:mm"), // Format start as "2025-02-14 14:00"
                            End = ((DateTime)reader["end"]).ToString("yyyy-MM-dd HH:mm") // Format end as "2025-02-14 15:00"
                        };
                        timesheetDetails.Add(details);
                    }
                }
            }
        }

        return timesheetDetails;
    }

    public Timesheet GetTimesheetById(int timesheetId)
{
    Timesheet timesheet = null;

    using (var connection = new SqlConnection(_connectionString))
    {
        connection.Open();  // Open the database connection

        using (var command = new SqlCommand("GetTimesheetById", connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TimesheetId", timesheetId);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    timesheet = new Timesheet
                    {
                        Id = (int)reader["id"],
                        EmployeeId = (int)reader["employee_id"],
                        StartTime = (DateTime)reader["start_time"],
                        EndTime = (DateTime)reader["end_time"],
                        WorkSummary = reader["work_summary"].ToString()
                    };
                }
            }
        }
    }

    return timesheet;
}
    public bool UpdateTimesheet(int timesheetId, Timesheet timesheet)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new SqlCommand("UpdateTimesheet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TimesheetId", timesheetId);
                command.Parameters.AddWithValue("@EmployeeId", timesheet.EmployeeId);
                command.Parameters.AddWithValue("@StartTime", timesheet.StartTime);
                command.Parameters.AddWithValue("@EndTime", timesheet.EndTime);
                command.Parameters.AddWithValue("@WorkSummary", timesheet.WorkSummary);

                int rowsAffected = command.ExecuteNonQuery(); // Get the number of affected rows
                return rowsAffected > 0; // Return true if any rows were updated
            }
        }
    }



}
