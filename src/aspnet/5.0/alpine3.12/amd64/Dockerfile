ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-rtm.20520.6

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-5.0.0-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='32776facab3782c8723e3df7fd2df68a45d6e5cc7c964a00eff82e85a1caa2c47ca2c2cfc1595161d4c0cfca449a5eaf0518ab3aeafc7585720fd3e6db0a63a7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
