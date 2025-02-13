var builder = WebApplication.CreateBuilder(args);

// Register EmployeeService and TimeSheetService
builder.Services.AddSingleton<EmployeeRepository>(sp =>
    new EmployeeRepository(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<TimesheetRepository>(sp =>
    new TimesheetRepository(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy for React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        corsBuilder.WithOrigins("http://localhost:3000")  // Allow your React app URL
                   .AllowAnyMethod()
                   .AllowAnyHeader();
    });
});

// Build the application
var app = builder.Build();

// Enable CORS for all requests
app.UseCors("AllowAll");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
