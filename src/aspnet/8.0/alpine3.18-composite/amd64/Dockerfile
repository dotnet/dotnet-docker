ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:8.0.0-rc.2-alpine3.18-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.0-rc.2.23479.6 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.0-rc.2.23480.2


# Install ASP.NET Composite Runtime
RUN wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-composite-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='c779a171e9a10ced7990ebb6d157e9bbaa6f777e79c2afcbd4dd4dbe857ac41e60dd9ce9252cf48aefa8e41449ca071b0e0c85e90670fd9c54cac3755e470626' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
