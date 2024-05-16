using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(p => p.AddPolicy("corsenabled", options =>
{
    options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// Add services to the container.
string conn = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddTransient<PersonajesRepository>();
builder.Services.AddDbContext<TelevisionContext>(options => options.UseMySql(conn, ServerVersion.AutoDetect(conn)));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Api Personajes AWS",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(optionns =>
{
    optionns.SwaggerEndpoint("/swagger/v1/swagger.josn", "Api Personajes AWS");
    optionns.RoutePrefix = "";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("corsenabled");
app.UseAuthorization();
app.MapControllers();

app.Run();
