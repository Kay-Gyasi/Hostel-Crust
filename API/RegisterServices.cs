namespace API
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddMemoryCache();

            builder.Services.AddSwaggerGen(c => c.CustomOperationIds(apiDescription =>
            {
                return apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
            }));

            builder.Services.AddDbContext<HostelContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("HostelCrust"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //var secretKey = configuration.GetSection("AppSettings:HostelCrustKey").Value;
            var secretKey = builder.Configuration.GetSection("HostelCrustKey").Value;

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
                        builder.WithOrigins("https://hostelcrust.vercel.app", "https://hostel-crust-admin.vercel.app",
                            "https://localhost:7189/")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
        }
    }
}
