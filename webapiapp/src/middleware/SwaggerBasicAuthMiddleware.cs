

using System.Net.Http.Headers;
using System.Text;

public class SwaggerBasicAuthMiddleware
{
    private readonly RequestDelegate next;
    private readonly IConfiguration configuration;

    public SwaggerBasicAuthMiddleware(RequestDelegate Next,IConfiguration configuration)
    {
        next = Next;
        this.configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            var authHeader=context.Request.Headers.Authorization.ToString();

             if (string.IsNullOrEmpty(authHeader))
            {
                await Challenge(context);
                return;
            }

            var auth =AuthenticationHeaderValue.Parse(authHeader);
            if (auth.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                var credentials=Encoding.UTF8.GetString(Convert.FromBase64String(auth.Parameter!));
                var parts=credentials.Split(':',2);
                if (parts.Length==2)
                {
                    var username=parts[0];
                    var password=parts[1];
                    // valid password
                    var validUsername=configuration["swaggerAuth:Username"];
                    var validPassword=configuration["swaggerAuth:Password"];

                    if (username==validUsername && password==validPassword)
                    {
                        await next(context);
                        return;
                    }
                }
            }
            
            await Challenge(context);
            return;
        }
    
        await next(context);
    }

    private static async Task Challenge(HttpContext context)
    {
        context.Response.StatusCode=StatusCodes.Status401Unauthorized;
        context.Response.Headers.WWWAuthenticate="Basic realm=\"swagger\"";
        await context.Response.WriteAsync("Unauthorize");
    }
}