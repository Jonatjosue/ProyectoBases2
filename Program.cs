using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using proyectoMMSBS2;
using proyectoMMSBS2.DTOs;
using proyectoMMSBS2.Servicios;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddCors(options =>
    options.AddPolicy("frontendControler", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
    )
);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<autenticacionJwt>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("frontendControler");

app.UseAuthorization();

app.MapControllers();

app.Run();
