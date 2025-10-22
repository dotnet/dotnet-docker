ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-alpine3.22-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0 \
    && aspnetcore_build_version=$aspnetcore_version-rtm.25521.107 \
    && wget \
        https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_build_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
        https://ci.dot.net/public-checksums/aspnetcore/Runtime/$aspnetcore_build_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz.sha512 \
    && echo "$(cat aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz.sha512)  aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
        aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz.sha512


# ASP.NET Core image
FROM $REPO:10.0.0-alpine3.22-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
