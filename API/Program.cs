var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI(c => {

    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerceApi v1");
    //c.RoutePrefix = String.Empty;
    c.DisplayOperationId();
});

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
