

public static class Startup
{
    public static void AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
    public static void UseAppServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapGet("/", () => "welcome to cj webapiapp");
        app.MapControllers();
    }
}