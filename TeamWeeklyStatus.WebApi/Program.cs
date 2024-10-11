using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.Services;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamWeeklyStatus.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot";
});

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
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
builder.Services.AddScoped<IWeeklyStatusRepository, WeeklyStatusRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<IWeeklyStatusService, WeeklyStatusService>();
builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();

var googleClientId = builder.Configuration["GoogleClientId"];
var googleClientSecret = builder.Configuration["GoogleClientSecret"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/";
    })
    .AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
