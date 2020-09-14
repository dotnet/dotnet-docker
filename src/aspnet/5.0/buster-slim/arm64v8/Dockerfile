ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=5.0.0-rc.2.20460.25

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='f461b469f8b09ec122d8b03bfc4a373f031f7ee177afb245ef3610c6a03f0f8c8527cbf128239cb3979b063e7a97422039d1eac590e27e26f29b245d5d59c922' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-buster-slim-arm64v8
ARG ASPNET_VERSION

ENV ASPNET_VERSION $ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
