using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ? ����� CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("https://miryam-taxadvisor.netlify.app", "http://localhost:4200")

                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});
// ? ����� ������� (Controllers + Swagger)
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

var app = builder.Build();

// ? Middleware - Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = "swagger"; // ���� ����� �� ����� �� ����
});
// ? Middleware - ����� ������ + CORS


app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true, // ���� ����� �� ���� ������
    DefaultContentType = "application/octet-stream" // ����� ����
    ,
    OnPrepareResponse = ctx =>
    {
        var origin = ctx.Context.Request.Headers["Origin"].ToString();
        if (origin == "http://localhost:4200" || origin == "https://miryam-taxadvisor.netlify.app")
        {
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        }
    }
});
app.UseCors("AllowAll");
// ? Middleware - Routing + Authorization
app.UseRouting();
app.UseAuthorization();

// ? ����� ������
app.MapControllers();

//app.Run();
app.Run("http://0.0.0.0:8080"); // Ensure the app listens on 8080
