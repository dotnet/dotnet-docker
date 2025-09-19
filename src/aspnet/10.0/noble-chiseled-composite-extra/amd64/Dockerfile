ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-rc.2.25468.104 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz.sha512 \
    && echo "$(cat aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz.sha512)  aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz --directory /usr/share/dotnet \
    && rm \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz.sha512

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.0-rc.2-noble-chiseled-extra-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-rc.2.25468.104 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-rc.2.25468.104

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
