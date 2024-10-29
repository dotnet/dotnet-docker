ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:9.0.0-alpine3.20-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rtm.24516.19

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-9.0.0-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='efaa7185f7410d2fda252ab62406125416cf49512890c12dfc0817d9a107055028d08b0361afaae2f950149254cae6698c24a5b72acdae8d69e3a48e1fa5d613' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
