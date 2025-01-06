ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:8.0.11-alpine3.20-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.11

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://builds-int.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='862ca7cf349e9454203a1389ab85283c91a104d7d6b70ae66c39b7d413a351df2075edba520673153110b9ebad15801b6b2284dafb22bdaf93555b964367df40' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
