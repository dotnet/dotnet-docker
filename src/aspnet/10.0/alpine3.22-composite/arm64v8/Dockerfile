ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.5-alpine3.22-arm64v8 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.5.25277.114 \
    && wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='246f726e68beacceb8b96177663bb2d6b00153ed51bfe591234f5f59f9c8e50b4df3c8e8084c52d81ff144d47fb78bd7c40e7c9bdd51b7dd4ee51aa5ed462f30' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.5-alpine3.22-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.5.25277.114 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.5.25277.114

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
