

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
            app.UseSwagger();
            app.UseSwaggerUI();
        
        // app.UseHttpsRedirection();
        app.UseAuthorization();
        // ui to login before the index route of the api runs, so that the user can login before accessing the api
        app.MapGet("/", () => "welcome to cj webapiapp");
        app.MapControllers();
    }
}