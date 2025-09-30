ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-rc.2-alpine3.22-arm64v8 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-rc.2.25479.113 \
    && wget \
        https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz \
        https://ci.dot.net/public-checksums/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz.sha512 \
    && echo "$(cat aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz.sha512)  aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz --directory /dotnet \
    && rm \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz.sha512


# ASP.NET Composite Image
FROM $REPO:10.0.0-rc.2-alpine3.22-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-rc.2.25479.113 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-rc.2.25479.113

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
