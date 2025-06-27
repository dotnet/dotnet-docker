ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.17 \
    && curl --fail --show-error --location \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/checksums/8.0.17-sha.txt \
    && sha512sum -c 8.0.17-sha.txt --ignore-missing \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
        8.0.17-sha.txt


# ASP.NET Core image
FROM $REPO:8.0.17-noble-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
