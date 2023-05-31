ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.0-preview.6-alpine3.17-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.0-preview.6.23280.5 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.0-preview.6.23280.11


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='3b676bdc7b395c5e02797161af64bdbdaf2f6f7b96cfa2b1f9bb5bba793493bb88d975917f5932053545e6bf38c095949209540c516bff28ce19c2e2cf93be5e' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

