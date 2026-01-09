ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.1 \
    && curl --fail --show-error --location \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz.sha512 \
    && sha512sum -c aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz.sha512 \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
        aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz.sha512


# ASP.NET Core image
FROM $REPO:10.0.1-noble-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
