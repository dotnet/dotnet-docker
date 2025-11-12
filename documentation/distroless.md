# Distroless .NET Images

.NET "distroless" container images contain only the minimal set of packages .NET needs, with everything else removed.
Due to their limited set of packages, distroless containers have a minimized security attack surface, smaller deployment sizes, and faster start-up time compared to their non-distroless counterparts.
They contain the following features:

- Minimal set of packages required for .NET applications
- Non-root user by default
- No package manager
- No shell

We offer distroless .NET images for two operating systems: [Ubuntu Chiseled](./ubuntu-chiseled.md) and [Azure Linux](./azurelinux.md).

.NET distroless images are available for the following image repos:

- [`mcr.microsoft.com/dotnet/runtime`](../README.runtime.md)
- [`mcr.microsoft.com/dotnet/aspnet`](../README.aspnet.md)
- [`mcr.microsoft.com/dotnet/runtime-deps`](../README.runtime-deps.md) (for self-contained or AOT apps)

We’re not offering distroless SDK images at the moment as there hasn't been a strong need for one, and because they may be difficult to use to use for some scenarios due to the lack of a shell.
You can use the existing SDK images to build your apps to run on distroless base images.
If you have a compelling use case for a distroless SDK image, please leave a comment on [this issue](https://github.com/dotnet/dotnet-docker/issues/4942) and we’ll be happy to reconsider.

## FAQs

### How do I use globalization with distroless images?

Our distroless images are focused on size. That means the default distroless images do not include the `icu` or `tzdata`
libraries. However, we offer an `extra` image variant that includes `tzdata` and `icu` by default.
You can use this in place of the default distroless images by appending the `-extra` suffix to the image tag like so:

**Ubuntu**:

- `mcr.microsoft.com/dotnet/runtime-deps:10.0-noble-chiseled-extra`
- `mcr.microsoft.com/dotnet/runtime:10.0-noble-chiseled-extra`
- `mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra`

**Azure Linux**:

- `mcr.microsoft.com/dotnet/runtime-deps:10.0-azurelinux3.0-extra`
- `mcr.microsoft.com/dotnet/runtime:10.0-azurelinux3.0-extra`
- `mcr.microsoft.com/dotnet/aspnet:10.0-azurelinux3.0-extra`

Please see ["Announcement: New approach for differentiating .NET 8+ images"](https://github.com/dotnet/dotnet-docker/discussions/4821) for more info.

### How can I scan distroless images for security vulnerabilities?

Both Ubuntu Chiseled and Azure Linux .NET images contain package installation data and are supported by many major scanning tools such as Qualys, Trivy, Syft, etc.
For example, you can scan for CVEs with [Docker Scout](https://docs.docker.com/scout/) using the following command:

**Ubuntu**:

```bash
docker scout cves mcr.microsoft.com/dotnet/runtime-deps:10.0-noble-chiseled
```

**Azure Linux**:

```bash
docker scout cves mcr.microsoft.com/dotnet/runtime-deps:10.0-cbl-mariner2.0-distroless
```

### How do I write my Dockerfile to work without a shell?

If you switch your containers to distroless images, you may run into one of the following errors:

- `stat  /bin/sh: no such file or directory`
- `docker: Error response from daemon: failed to create shim task: OCI runtime create failed: runc create failed: unable to start container process: exec: "bash": executable file not found in $PATH: unknown.`

These errors happen when you try to invoke a shell command in a container that doesn't include `sh` or `bash`.
To avoid these, you'll need to make sure your Dockerfile is architected correctly and your .NET app doesn't depend on the shell.

First, use the `exec` form instead of the `shell` form for instructions in your Dockerfile's distroless stage. For example:

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
FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
# OR
# FROM mcr.microsoft.com/dotnet/sdk:10.0-azurelinux3.0 AS build
...
RUN wget -O somefile.tar.gz <URL> \
    && tar -oxzf aspnetcore.tar.gz -C /somefile-extracted
...

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-noble-chiseled
# OR
# FROM mcr.microsoft.com/dotnet/sdk:10.0-azurelinux3.0-distroless AS build
...
COPY --from=build /somefile-extracted .
...
```

### How do I handle file permissions when running as a non-root user?

If your app writes to the disk, you may encounter permission issues at runtime, such as:

```text
System.UnauthorizedAccessException: Access to the path "<path>" is denied
```

Distroless .NET images use a non-root user by default.
This error happens when apps try to write to a directory or file that the current user doesn't have access to.
By design, an app shouldn't be able to modify the directory it's running from. Instead, you should try to write to the non-root user's home directory.

For example, instead of:

```cs
File.WriteAllLines("myFile.txt", myText);
```

Try writing to the user's home directory:

```cs
string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "myFile.txt");
File.WriteAllLines(path, myText);
```

If you are writing to bind or volume mounted directories, you will need to make sure the directory gives the correct permissions to the non-root user.
You can check the user ID that a container will use by running the following command:

**Ubuntu**:

```console
docker image inspect mcr.microsoft.com/dotnet/runtime-deps:10.0-noble-chiseled -f "{{ .Config.User }}"
```

**Azure Linux**:

```console
docker image inspect mcr.microsoft.com/dotnet/runtime-deps:10.0-azurelinux3.0-distroless -f "{{ .Config.User }}"
```

Additionally, from within the container, you can get the UID of the non-root user by reading the `APP_UID` environment variable.
