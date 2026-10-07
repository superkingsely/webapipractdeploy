### Step 13 — Make the Docker container Render-ready

Change the bottom of your Dockerfile from:

```dockerfile
EXPOSE 8080

ENTRYPOINT ["dotnet", "webapiapp.dll"]
```

to:

```dockerfile
ENV ASPNETCORE_HTTP_PORTS=10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "webapiapp.dll"]
```

So your complete Dockerfile is:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY webapiapp/ webapiapp/
COPY infrastructure/ infrastructure/

RUN dotnet restore "webapiapp/webapiapp.csproj"

COPY . .

WORKDIR /src/webapiapp

RUN dotnet build "webapiapp.csproj" -c Release -o /app/build

RUN dotnet publish "webapiapp.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "webapiapp.dll"]
```

Render supports Dockerfiles directly and uses the Dockerfile's runtime configuration when deploying. ([Render][1])

---

## Now let's test the image locally

**Don't push to Render yet.** We want to prove that your Dockerfile works on your machine first.

From:

```bash
~/Desktop/my-work/webapipractdeploy
```

run:

```bash
docker build -t webapipract-api .
```

### What this command means

```text
docker build
     │
     ├── -t webapipract-api
     │       ↓
     │    give image a name
     │
     └── .
         ↓
      use current directory
      as the build context
```

Docker should then go through:

```text
FROM .NET SDK
      ↓
WORKDIR
      ↓
COPY
      ↓
dotnet restore
      ↓
COPY
      ↓
dotnet build
      ↓
dotnet publish
      ↓
FROM ASP.NET Runtime
      ↓
COPY published application
      ↓
IMAGE CREATED
```

You should eventually see something like:

```text
Successfully tagged webapipract-api:latest
```

or the newer BuildKit equivalent showing that the image was successfully built.

### Run this now:

```bash
docker build -t webapipract-api .
```

**Don't run `docker run` yet.** Send me the build output if you get an error; if it succeeds, we'll run the container and test your actual API before touching Render.

[1]: https://render.com/docs/docker?utm_source=chatgpt.com "Docker on Render – Render Docs"
