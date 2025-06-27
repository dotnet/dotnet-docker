ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.6-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.6 \
    && wget \
        https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/checksums/9.0.6-sha.txt \
    && awk -v file="aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz" '{gsub(/\r/, "")} $2 == file' 9.0.6-sha.txt | sha512sum -c \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
        9.0.6-sha.txt


# ASP.NET Core image
FROM $REPO:9.0.6-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
