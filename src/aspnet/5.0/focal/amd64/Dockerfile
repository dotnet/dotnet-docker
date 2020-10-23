ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=5.0.0-rtm.20520.6

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-5.0.0-linux-x64.tar.gz \
    && aspnetcore_sha512='11d2ff5e4be23d4a8e3b67e669100f83cf5daf28a77a3888e918b19206777f6a62368028f8a1fb1bd5801b3c4a3db749e09b31fb8435e03a21caaaf475c7ac1d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-focal-amd64
ARG ASPNET_VERSION

ENV ASPNET_VERSION $ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
