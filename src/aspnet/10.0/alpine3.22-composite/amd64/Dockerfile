ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.5-alpine3.22-amd64 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.5.25277.114 \
    && wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='197d5a9c080743a7ce1f3547bd7a3d7abce2a1f66796416fabdd01630bb127033004013eab1cfb8cdeb1f80c62fe18fb858e13fd32388d2b48ea291a42c9c6f1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.5-alpine3.22-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.5.25277.114 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.5.25277.114

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
