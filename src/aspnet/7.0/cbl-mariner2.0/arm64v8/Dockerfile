ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22301.14

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='fe994c1d4ca12392c8983dc8a063f7c7d491a67d647ffd5cef9a2a4f87784e7fe6488a8cc63ca0482d8da4d94d9accac802f3147e9459ee44a1c6bff9810417e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
