ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.17-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.17 \
    && wget \
        https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/checksums/8.0.17-sha.txt \
    && awk -v file="aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz" '{gsub(/\r/, "")} $2 == file' 8.0.17-sha.txt | sha512sum -c \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
        8.0.17-sha.txt


# ASP.NET Core image
FROM $REPO:8.0.17-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
