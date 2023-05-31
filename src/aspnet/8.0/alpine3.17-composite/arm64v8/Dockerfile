ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.0-preview.6-alpine3.17-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.0-preview.6.23280.5 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.0-preview.6.23280.11


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && dotnet_sha512='35ce19cdcd10e6f648f119c408f2069b6c97f07201787e6d80c30522ea349cc5c28a63202892d6afe4f55efff7fcafb833fd255567110dffd940e53582de616a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

