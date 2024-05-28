ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.0-preview.5-alpine3.19-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-preview.5.24272.2 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-preview.6.24277.5


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='5e0873f9858a3ad124671365ae967061aab992541ea648a9df4c9e855851224a14f2c12cf9bead30f68eb8e9406423770a2aac5f4388363ead3ba166b251d79e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
