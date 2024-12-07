ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:9.0.0-alpine3.21-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && dotnet_sha512='106457de6f34a2996923a77589e841815239a760382ee525c3b714f9e6f65039d8555a5e371aaad0e89c02f9dfa5bb267cac22fbc1cd383f696facebe5b34a97' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
