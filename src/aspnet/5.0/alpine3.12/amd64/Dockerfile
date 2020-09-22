ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-rc.2.20471.23

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='df774ec1b2f6a34eb99dabf97d97a226ac2eb3d6b872c0936ed6dc42057fe698754a27c95327e86f41f12c3bf234fbf3b30238fef3634db907293266e1e1062a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
