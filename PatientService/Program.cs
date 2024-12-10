using PatientService.Data;
using Microsoft.EntityFrameworkCore;
using PatientService.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using PatientService.Services;
using AMS.Common.Middleware.Extensions;
using Serilog.Sinks.PostgreSQL;
using Serilog;
using Npgsql;
using NpgsqlTypes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My patient service API", Version = "v1" });

    // Define the BearerAuth scheme that's in use
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
            });
});

// Configure DbContext with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MediatR for CQRS pattern
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

// Register repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
// Add JWT authentication for auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddSingleton<TestUnstableService>();
builder.Services.AddSingleton<CircuitBreakerService>();

builder.Services.AddAuthorization();

// Read connection string from appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Define the columns and their types for logging
var columnWriters = new Dictionary<string, ColumnWriterBase>
{
    { "Timestamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
    { "Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
    { "Message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
    { "Exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
    { "Properties", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) }
};

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.PostgreSQL(
        connectionString: connectionString,
        tableName: "Logs",
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            { "Timestamp", new TimestampColumnWriter(NpgsqlTypes.NpgsqlDbType.Timestamp) },
            { "Level", new LevelColumnWriter(true, NpgsqlTypes.NpgsqlDbType.Varchar) },
            { "Message", new RenderedMessageColumnWriter(NpgsqlTypes.NpgsqlDbType.Text) },
            { "Exception", new ExceptionColumnWriter(NpgsqlTypes.NpgsqlDbType.Text) },
            { "Properties", new PropertiesColumnWriter(NpgsqlTypes.NpgsqlDbType.Jsonb) }
        })
    .CreateLogger();

builder.Host.UseSerilog();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

// Order matters: Logging before Exception Handling
app.UseRequestLogging();
app.UseGlobalExceptionHandling();

app.MapControllers();

app.Run();
