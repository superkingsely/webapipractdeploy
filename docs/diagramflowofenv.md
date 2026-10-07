                 Render
                   │
       Environment Variables
                   │
       ┌───────────┴───────────┐
       │                       │
 Jwt__Secret             ConnectionStrings__...
       │                       │
       └───────────┬───────────┘
                   ▼
             Docker Container
                   │
                   ▼
             ASP.NET Core
                   │
                   ▼
             IConfiguration
==========================
Dockerfile
    ↓
Build application

Render
    ↓
Inject secrets/environment variables