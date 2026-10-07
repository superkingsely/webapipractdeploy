Perfect. 🎉 Now we verify Docker before touching your project.

### 1. Start Docker Desktop

Open **Docker Desktop** from Windows Start.

Wait until Docker Desktop shows that the engine is **running/ready**.

### 2. Open a fresh Git Bash

Don't use the old Git Bash window from before the installation. Open a new one.

Run:

```bash
docker --version
```

You should get something like:

```text
Docker version 29.x.x, build ...
```

Then run the important test:

```bash
docker run hello-world
```

The first time, Docker may download the `hello-world` image. That's normal.

You should eventually see:

```text
Hello from Docker!
```

### 3. Verify WSL integration

In PowerShell, run:

```powershell
wsl -l -v
```

We want:

```text
NAME      STATE      VERSION
Ubuntu    Running    2
```

or `Stopped 2`.

---

### After these tests

Don't create the Dockerfile yet.

Once the three checks work:

```bash
docker --version
docker run hello-world
```

and:

```powershell
wsl -l -v
```

we'll start with **your actual `webapipractdeploy` project**.

I'll teach you Docker in this order:

```text
1. What Docker actually is
2. Image vs Container
3. Dockerfile syntax
4. FROM
5. WORKDIR
6. COPY
7. RUN
8. dotnet restore
9. dotnet build
10. dotnet publish
11. EXPOSE
12. ENTRYPOINT
13. Multi-stage builds
14. .dockerignore
15. Build your image
16. Run your .NET API container
17. Test your API
18. Push Dockerfile to GitHub
19. Deploy to Render
```

**Start with these two commands in a new Git Bash:**

```bash
docker --version
```

```bash
docker run hello-world
```

Send me the output.
