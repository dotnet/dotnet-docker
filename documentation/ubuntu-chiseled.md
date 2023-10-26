# Ubuntu Chiseled + .NET

## What is Ubuntu Chiseled?

.NET's Ubuntu Chiseled images are a type of "distroless" container image that contain only the minimal set of packages .NET needs, with everything else removed.
These images offer dramatically smaller deployment sizes and attack surface through the following features:

- Ultra-small images (reduced size and attack surface)
- No package manager (avoids a whole class of attacks)
- No shell (avoids a whole class of attacks)
- Non-root by default (avoids a whole class of attacks)

Ubuntu Chiseled images are available for the following image repos:
- [`mcr.microsoft.com/dotnet/runtime`](https://mcr.microsoft.com/product/dotnet/runtime/about)
- [`mcr.microsoft.com/dotnet/aspnet`](https://mcr.microsoft.com/product/dotnet/aspnet/about)
- [`mcr.microsoft.com/dotnet/runtime-deps`](https://mcr.microsoft.com/product/dotnet/runtime-deps/about) (for self-contained apps)

We’re not offering a chiseled SDK image as there wasn't a strong need for one, and a chiseled SDK image could be hard to use for some scenarios.
You can continue to use the existing Ubuntu SDK images to build your apps to run on Chiseled.
If you have a compelling use case for a distroless SDK image, please [file a discussion](https://github.com/dotnet/dotnet-docker/discussions/new/choose) and we’ll be happy to reconsider.

## How do I use Ubuntu Chiseled .NET images?

Please see our sample Dockerfiles for examples on how to use Ubuntu Chiseled .NET images:
- [aspnetapp](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/Dockerfile.chiseled)
- [dotnetapp](https://github.com/dotnet/dotnet-docker/blob/main/samples/dotnetapp/Dockerfile.chiseled)
- [releasesapi](https://github.com/dotnet/dotnet-docker/blob/main/samples/releasesapi/Dockerfile.ubuntu-chiseled) (and [icu version](https://github.com/dotnet/dotnet-docker/blob/main/samples/releasesapi/Dockerfile.ubuntu-chiseled-icu))
- [releasesapp](https://github.com/dotnet/dotnet-docker/blob/main/samples/releasesapp/Dockerfile.chiseled)

If your app's Dockerfile doesn't install any additional linux packages or depend on any shell scripts for setup, Ubuntu Chiseled images could be a drop-in replacement for our default Ubuntu images.

## FAQs

### How do I use globalization with Chiseled images?

Our Chiseled images are focused on size. That means the default Chiseled images do not include the `icu` or `tzdata`
libraries from Ubuntu. However, we offer an `extra` image variant that includes `tzdata` and `icu` by default. You can
use this in place of the default chiseled image appending the `-extra` suffix like so:

- `8.0-jammy-chiseled-extra`

Please see ["Announcement: New approach for differentiating .NET 8+ images"](https://github.com/dotnet/dotnet-docker/discussions/4821) for more info.

### How can I scan Chiseled images for security vulnerabilities?

Ubuntu Chiseled .NET images maintain the `dpkg` cache file, so they are supported by all major scanning tools: Docker Scout, Trivy, Syft, etc.
For example, you can scan for CVEs with [Docker Scout](https://docs.docker.com/scout/) using the following command:

```bash
docker scout cves mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled
```

## Common issues adopting distroless containers

### Trying to invoke a shell

If you switch your containers to Ubuntu Chiseled, you may run into one of the following errors:

- `stat  /bin/sh: no such file or directory`
- `docker: Error response from daemon: failed to create shim task: OCI runtime create failed: runc create failed: unable to start container process: exec: "bash": executable file not found in $PATH: unknown.`

These errors happen when you try to invoke a shell Ubuntu Chiseled images since they don't include `sh` or `bash`.
Before adopting Ubuntu Chiseled, you should make sure your app doesn't depend on any shell commands or scripts.

Additionally, you should make sure any instructions in your Dockerfile are formatted in the `exec` form instead of the `shell` form. For example:

```Dockerfile
# "Exec" form - Works without a shell
RUN ["dotnet", "--list-runtimes"]
ENTRYPOINT ["dotnet", "myapp.dll"]
CMD ["dotnet", "myapp.dll", "--", "args"]

# "Shell" form - Doesn't work without a shell
RUN dotnet --list-versions
ENTRYPOINT dotnet myapp.dll
CMD dotnet myapp.dll --args
```

### Permission denied error

If your app writes to the disk, you may encounter permission issues at runtime, such as:

```
System.UnauthorizedAccessException: Access to the path "<path>" is denied
```

Ubuntu Chiseled .NET images use a non-root user by default.
This error happens when apps try to write to a directory that the current user doesn't have access to.
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
