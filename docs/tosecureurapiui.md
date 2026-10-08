
Yes 👍 Let's use **Basic Auth for Swagger**. This is much shorter and fits exactly what you want.

Our goal:

```text
http://localhost:10000/swagger
             ↓
      🔐 Browser popup
             ↓
Username + Password
             ↓
        Correct?
       ↙        ↘
     NO          YES
     ↓            ↓
   401         Swagger UI
```

And we won't touch ASP.NET Identity for this.

## Step 1 — Create the middleware

In your `webapiapp` project, create:

```text
src/
└── Middleware/
    └── SwaggerBasicAuthMiddleware.cs
```

Put this inside:

```csharp
using System.Net.Http.Headers;
using System.Text;

public class SwaggerBasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public SwaggerBasicAuthMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            var authHeader = context.Request.Headers.Authorization.ToString();

            if (string.IsNullOrEmpty(authHeader))
            {
                await Challenge(context);
                return;
            }

            var auth = AuthenticationHeaderValue.Parse(authHeader);

            if (auth.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                var credentials = Encoding.UTF8.GetString(
                    Convert.FromBase64String(auth.Parameter!)
                );

                var parts = credentials.Split(':', 2);

                if (parts.Length == 2)
                {
                    var username = parts[0];
                    var password = parts[1];

                    var validUsername = _configuration["SwaggerAuth:Username"];
                    var validPassword = _configuration["SwaggerAuth:Password"];

                    if (username == validUsername && password == validPassword)
                    {
                        await _next(context);
                        return;
                    }
                }
            }

            await Challenge(context);
            return;
        }

        await _next(context);
    }

    private static async Task Challenge(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers.WWWAuthenticate = "Basic realm=\"Swagger\"";

        await context.Response.WriteAsync("Unauthorized");
    }
}
```

### What this middleware does

The important part is:

```csharp
if (context.Request.Path.StartsWithSegments("/swagger"))
```

We're saying:

> **Only intercept requests going to `/swagger`.**

So:

```text
/swagger          🔐 protected
/swagger/index.html 🔐 protected
```

but your other routes aren't affected by this middleware.

Then:

```csharp
context.Response.Headers.WWWAuthenticate =
    "Basic realm=\"Swagger\"";
```

This is what tells the browser:

> **"I need Basic Authentication."**

That's what causes the browser's native username/password popup to appear.

---

## Step 2 — Put the credentials in configuration

In your `appsettings.json`, add:

```json
{
  "SwaggerAuth": {
    "Username": "admin",
    "Password": "ChangeThisPassword"
  }
}
```

**But don't use that real password in production.**

For local learning/testing, it's okay.

For Render, we'll put:

```text
SwaggerAuth__Username
SwaggerAuth__Password
```

into Render's environment variables instead.

That means your production password doesn't have to be committed to GitHub.

---

## Step 3 — Register the middleware

In your `UseAppServices()` method, add:

```csharp
app.UseMiddleware<SwaggerBasicAuthMiddleware>();
```

**before Swagger is served.**

Since your current Swagger is inside:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

we'll place the middleware before that block:

```csharp
public static void UseAppServices(this WebApplication app)
{
    app.UseMiddleware<SwaggerBasicAuthMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapGet("/", () => "welcome to cj webapiapp");

    app.MapControllers();
}
```

### One thing though ⚠️

There's a small improvement I want us to make **before you paste this and run it**: your Swagger is currently **Development-only**.

So on your local machine:

```text
Development
   ↓
Swagger exists
   ↓
Basic Auth protects it
```

But in Render:

```text
Production
   ↓
Swagger doesn't exist
```

If your goal is to **login before seeing Swagger in both Development and Production**, we'll need to change that part too.

I suggest we do that **next**, before testing, rather than mixing several changes at once.
