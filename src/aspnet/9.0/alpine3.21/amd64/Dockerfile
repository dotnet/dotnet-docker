ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.6-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.6 \
    && wget --output-document aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='db67082a6bd12d268dfb393dac12df316268e52401066e46aed21adcc55a69d3102d57a0930bd5b0e5a20e5abcf64d49eca99518b2ac912d779a0047d36c45be' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum --check - \
    && mkdir -p /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.6-alpine3.21-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
