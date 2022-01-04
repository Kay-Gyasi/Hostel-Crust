using API.Data.Unit_Of_Work;
using API.Interfaces;
using Data_Layer.Data_Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options => 
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => c.CustomOperationIds(apiDescription =>
{
    return apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
}));

builder.Services.AddDbContext<HostelContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var secretKey = builder.Configuration.GetSection("AppSettings:Key").Value;

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false, // these two will be set to true upon deployment
            IssuerSigningKey = key
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.WithOrigins("https://hostelcrust.vercel.app/", "https://hostel-crust-admin.vercel.app/")
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI(c => {

    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerceApi v1");
    c.RoutePrefix = String.Empty;
    c.DisplayOperationId();
});

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
