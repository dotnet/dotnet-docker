ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.22-alpine3.23-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.22 \
    && wget \
        https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/checksums/$aspnetcore_version-sha.txt \
    && awk -v file="aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz" '{gsub(/\r/, "")} $2 == file' $aspnetcore_version-sha.txt | sha512sum -c \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm \
        aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
        $aspnetcore_version-sha.txt


# ASP.NET Core image
FROM $REPO:8.0.22-alpine3.23-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.22

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
