ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.6-alpine3.21-arm64v8 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.6 \
    && wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='7f411c68d72a0ff54263c15f3665747dc979b519401b893117bab5baa5599af51cc02d2679bb9a7b8f25888021b899557179a2d0b6f87dbe6a251581770185f7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# ASP.NET Composite Image
FROM $REPO:9.0.6-alpine3.21-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.6 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
