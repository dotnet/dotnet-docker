ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.6-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.6 \
    && wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='4b36300b9d40feb9de34468898780c4f1a38f2bacb2664ab3fcba11aa55282e329cba0deb09704c0ff5f4e0d713000533fcaf8f0a34ac4cde4833bb3224de782' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# ASP.NET Composite Image
FROM $REPO:9.0.6-alpine3.21-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.6 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
