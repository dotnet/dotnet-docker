ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:resolute-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=11.0.0-preview.1.26104.118 \
    && curl --fail --show-error --location \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz.sha512 \
    && sha512sum -c aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz.sha512 \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
        aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz.sha512


# ASP.NET Core image
FROM $REPO:11.0.0-preview.1-resolute-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=11.0.0-preview.1.26104.118

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
