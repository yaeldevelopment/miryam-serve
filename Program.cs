using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// הגדרת מדיניות CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
                "https://miryam-taxadvisor.netlify.app",
                "http://localhost:4200",
                "https://yaelajami.netlify.app")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

var app = builder.Build();

// Middleware - סדר חשוב מאוד
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = "swagger"; // מגדיר ש-Swagger UI יהיה בכתובת /swagger
});
// תמיכה בבקשות OPTIONS ל־CORS
app.Use(async (context, next) =>
{
    if (context.Request.Method == HttpMethods.Options)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});
// קבצים סטטיים
app.UseStaticFiles();

// Routing
app.UseRouting();

// 🟢 CORS חייב לבוא אחרי UseRouting ולפני Authorization
app.UseCors("AllowAll");

// הרשאות (אם אין לך Authentication, אפשר להסיר את זה)
app.UseAuthorization();

// מיפוי קונטרולרים
app.MapControllers();

// מאפשר לכל הפניות להגיע לכתובת הזו
app.Run("http://0.0.0.0:8080");
