ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=11.0.0-alpha.1 \
    && aspnetcore_build_version=$aspnetcore_version.25609.108 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_build_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/aspnetcore/Runtime/$aspnetcore_build_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz.sha512 \
    && echo "$(cat aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz.sha512)  aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
        aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz.sha512


# ASP.NET Core image
FROM $REPO:11.0.0-alpha.1-noble-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=11.0.0-alpha.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
