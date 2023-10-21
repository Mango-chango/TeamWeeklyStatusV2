using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.Services;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure;
using TeamWeeklyStatus.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var sqlServerConnectionString = builder.Configuration.GetConnectionString("AzureSqlConnection");

builder.Services.AddDbContext<TeamWeeklyStatusContext>(
    options => options.UseSqlServer(sqlServerConnectionString)
);

builder.Services.AddSingleton<
    IDesignTimeDbContextFactory<TeamWeeklyStatusContext>,
    TeamWeeklyStatusContextFactory
>();

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository<Team>, Repository<Team>>();
builder.Services.AddScoped<IRepository<Member>, Repository<Member>>();
builder.Services.AddScoped<IRepository<TeamMember>, Repository<TeamMember>>();
builder.Services.AddScoped<IRepository<WeeklyStatus>, Repository<WeeklyStatus>>();
builder.Services.AddScoped<IRepository<DoneThisWeekTask>, Repository<DoneThisWeekTask>>();
builder.Services.AddScoped<IRepository<PlanForNextWeekTask>, Repository<PlanForNextWeekTask>>();
builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
builder.Services.AddScoped<IWeeklyStatusRepository, WeeklyStatusRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWeeklyStatusService, WeeklyStatusService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
