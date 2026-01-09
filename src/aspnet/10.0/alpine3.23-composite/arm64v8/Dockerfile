ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.1-alpine3.23-arm64v8 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.1 \
    && wget \
        https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz.sha512 \
    && sha512sum -c aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz.sha512 \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz --directory /dotnet \
    && rm \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz.sha512


# ASP.NET Composite Image
FROM $REPO:10.0.1-alpine3.23-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.1 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
