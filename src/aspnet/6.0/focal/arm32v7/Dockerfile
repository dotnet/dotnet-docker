ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=6.0.0-alpha.1.21072.5

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='97442292268248f848a7d49fb65f7e5960b7598e6bccdc3d8812fe23f48c640ae51963a4d00f603ffb678f9a72ac4e82f8a7d4cc939ba06f7190b87f7ae837e8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0-focal-arm32v7
ARG ASPNET_VERSION

ENV ASPNET_VERSION=$ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
