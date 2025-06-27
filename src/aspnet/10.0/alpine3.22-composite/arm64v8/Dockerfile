ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm64v8 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.6.25326.107 \
    && wget --output-document dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && dotnet_sha512='998225901cdaa09adf20a4bcc322ada2b92c558180af3415e6bc58eb0aa088dbb2b52ddad9a5cd7413e23b3ddda9e63750583c4650576213f6f6ea0a64579383' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.6.25326.107 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.6.25326.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
