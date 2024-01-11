# Ubuntu Chiseled + .NET

## What is Ubuntu Chiseled?

.NET's Ubuntu Chiseled images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface compared to our "full" Ubuntu images that are based on the Ubuntu base images. This is achieved through the following features:

- Minimal set of packages required to run a .NET application
- Non-root user by default
- No package manager
- No shell

Ubuntu Chiseled images are available for the following image repos:
- [`mcr.microsoft.com/dotnet/runtime`](../README.runtime.md)
- [`mcr.microsoft.com/dotnet/aspnet`](../README.aspnet.md)
- [`mcr.microsoft.com/dotnet/runtime-deps`](../README.runtime-deps.md) (for self-contained apps)

We’re not offering a chiseled SDK image as there wasn't a strong need for one, and a chiseled SDK image could be hard to use for some scenarios.
You can continue to use the existing full Ubuntu SDK images to build your apps to run on Chiseled.
If you have a compelling use case for a distroless SDK image, please leave a comment on [this issue](https://github.com/dotnet/dotnet-docker/issues/4942) and we’ll be happy to reconsider.

## How do I use Ubuntu Chiseled .NET images?

Please see our sample Dockerfiles for examples on how to use Ubuntu Chiseled .NET images:
- [aspnetapp](../samples/aspnetapp/Dockerfile.chiseled)
- [dotnetapp](../samples/dotnetapp/Dockerfile.chiseled)
- [releasesapi](../samples/releasesapi/Dockerfile.ubuntu-chiseled) (and [icu version](../samples/releasesapi/Dockerfile.ubuntu-chiseled-icu))
- [releasesapp](../samples/releasesapp/Dockerfile.chiseled)

If your app's Dockerfile doesn't install any additional Linux packages or depend on any shell scripts for setup, Ubuntu Chiseled images could be a drop-in replacement for our full Ubuntu or Debian images.

## FAQs

### How do I use globalization with Chiseled images?

Our Chiseled images are focused on size. That means the default Chiseled images do not include the `icu` or `tzdata`
libraries from Ubuntu. However, we offer an `extra` image variant that includes `tzdata` and `icu` by default. You can
use this in place of the default chiseled image by appending the `-extra` suffix to the image tag like so:

- `mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled-extra`
- `mcr.microsoft.com/dotnet/runtime:8.0-jammy-chiseled-extra`
- `mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled-extra`

Please see ["Announcement: New approach for differentiating .NET 8+ images"](https://github.com/dotnet/dotnet-docker/discussions/4821) for more info.

### How can I scan Chiseled images for security vulnerabilities?

Ubuntu Chiseled .NET images maintain the `dpkg` cache file, so they are supported by all major scanning tools: Docker Scout, Trivy, Syft, etc.
For example, you can scan for CVEs with [Docker Scout](https://docs.docker.com/scout/) using the following command:

```bash
docker scout cves mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled
```

### How do I write my Dockerfile to work without a shell?

If you switch your containers to Ubuntu Chiseled, you may run into one of the following errors:

- `stat  /bin/sh: no such file or directory`
- `docker: Error response from daemon: failed to create shim task: OCI runtime create failed: runc create failed: unable to start container process: exec: "bash": executable file not found in $PATH: unknown.`

These errors happen when you try to invoke a shell command in a container that doesn't include `sh` or `bash`.
To avoid these, you'll need to make sure your Dockerfile is architected correctly and your .NET app doesn't depend on the shell.

First, use the `exec` form instead of the `shell` form for instructions in your Dockerfile's Ubuntu Chiseled stage. For example:

```Dockerfile
# "Exec" form - Works without a shell
RUN ["dotnet", "--list-runtimes"]
ENTRYPOINT ["dotnet", "myapp.dll"]
CMD ["dotnet", "myapp.dll", "--", "args"]

# "Shell" form - Doesn't work without a shell
RUN dotnet --list-runtimes
ENTRYPOINT dotnet myapp.dll
CMD dotnet myapp.dll -- args
```

Please see Docker's [Dockerfile documentation](https://docs.docker.com/engine/reference/builder/#run) for more info on instruction formatting.

If you need to run any shell commands or other utilities at build time, you can do so in the build stage of the multi-stage Dockerfile and copy the results to the final stage.
For example, you could download and extract an archive so that the files will be available in your container.

```Dockerfile
# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
...
RUN wget -O somefile.tar.gz <URL> \
    && tar -oxzf aspnetcore.tar.gz -C /somefile-extracted
...

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled
...
COPY --from=build /somefile-extracted .
...
```

### How do I handle file permissions when running as a non-root user?

If your app writes to the disk, you may encounter permission issues at runtime, such as:

```
System.UnauthorizedAccessException: Access to the path "<path>" is denied
```

Ubuntu Chiseled .NET images use a non-root user by default.
This error happens when apps try to write to a directory or file that the current user doesn't have access to.
By design, an app shouldn't be able to modify the directory it's running from. Instead, you should try to write to the non-root user's home directory.

For example, instead of:

```cs
File.WriteAllLines("myFile.txt", myText);
```

Try writing to the user's home directory:

```cs
string path = Path.Combine(Environment.SpecialFolder.UserProfile, "myFile.txt");
File.WriteAllLines(path, myText);
```

If you are writing to bind or volume mounted directories, you will need to make sure the directory gives the correct permissions to the non-root user.
You can check the user ID that a container will use by running the following command:

```
docker image inspect mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled -f "{{ .Config.User }}"
```

Additionally, from within the container, you can get the UID of the non-root user by reading the `APP_UID` environment variable.
