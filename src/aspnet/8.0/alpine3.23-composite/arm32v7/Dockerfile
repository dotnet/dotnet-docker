ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:8.0.22-alpine3.23-arm32v7 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=8.0.22 \
    && wget \
        https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm.tar.gz \
        https://builds.dotnet.microsoft.com/dotnet/checksums/$aspnetcore_version-sha.txt \
    && awk -v file="aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm.tar.gz" '{gsub(/\r/, "")} $2 == file' $aspnetcore_version-sha.txt | sha512sum -c \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm.tar.gz --directory /dotnet \
    && rm \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm.tar.gz \
        $aspnetcore_version-sha.txt


# ASP.NET Composite Image
FROM $REPO:8.0.22-alpine3.23-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.22 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.22

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
