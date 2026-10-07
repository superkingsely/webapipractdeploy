tree /F
USER@superkingselyPC MINGW64 ~/Desktop/my-work/webapipractdeploy (main)
$ tree /F
bash: tree: command not found

No problem 👍. You're using Git Bash on Windows, and tree isn't installed in your Git Bash environment.
================================================above didnt work
Let's use commands that work there.

From your project root:
pwd

Then:

find . -maxdepth 3 -type f

If that produces too much output, use:

find . -maxdepth 3 -type f | grep -E '\.(csproj|sln|cs|json)$'
===================================================================

find . -maxdepth 3 -type f | grep -E '\.(csproj|sln|cs|json)$'   ===> worked
===================================================================