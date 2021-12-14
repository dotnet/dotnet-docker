ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.13-alpine3.14-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.13

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='0ea8a945bb1b663b8bf65708d6cfd6411aaf6ac8cc2ade34dfda160c331230694620b8b0abf80c4266fc9a2444300bf9b58906e40c30e7aff7a27291240ca583' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
