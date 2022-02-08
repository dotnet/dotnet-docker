ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.14-alpine3.15-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.14

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='fbde931a95063b3ca2bb264af8c3b19ef85ea4f5e2e09b6669a39f3c08c4eba43a89dd2c64bc9f4ffb49d41f678c6654d1b5c1dcc35430091773983f8bd97473' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
