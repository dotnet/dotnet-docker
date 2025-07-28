ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.7.25374.104 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz.sha512 \
    && echo "$(cat aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz.sha512)  aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
        aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz.sha512


# ASP.NET Core image
FROM $REPO:10.0.0-preview.7-noble-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.7.25374.104

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
