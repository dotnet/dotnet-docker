ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=5.0.0-rc.1.20427.7

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='8c11c1c4fd1bb9f8a17e53514c6078325c3e6f5396ccbe575cc13e19146d4a9a658a87afbef5f6ed5915faa1dd7b3d6d5399e97b4f687de74de5e60327762e90' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-focal-arm32v7
ARG ASPNET_VERSION

ENV ASPNET_VERSION $ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
