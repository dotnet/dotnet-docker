ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.6-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.6 \
    && wget --output-document dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='9f1e39593bd4846574435bb7f492e3b5930ce7c3eb064437635fa207a130ce2283ebea08284141191c7635c67ae4e791dac55e7560dec1f38695056323e83f64' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# ASP.NET Composite Image
FROM $REPO:9.0.6-alpine3.21-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.6 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
