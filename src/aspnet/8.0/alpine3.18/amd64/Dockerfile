ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:8.0.0-rc.2-alpine3.18-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-rtm.23468.24

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='488982050e16de99727c38971f77f69b3f6b6b20d7bdf58b66ce8bb870a23101bf6c6093d1efd4650a27f7060d6d0b875e84d4452b26782f3140e8eae30d0be6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
