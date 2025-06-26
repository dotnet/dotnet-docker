ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.5-alpine3.22-arm32v7 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.5.25277.114 \
    && wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='a012ab79894caf198fe725825c6973c818b0f5ccb6ba7d0f306b6741fd488e39a5e3d1dfd90ff5d2b100714d2f1f2262b59c97c1d360406d37431b109f483c42' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.5-alpine3.22-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.5.25277.114 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.5.25277.114

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
