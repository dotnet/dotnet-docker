ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:10.0.0-alpha.1-alpine3.21-arm32v7

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-alpha.2.25070.3

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='4ad6f0b2ac4f0fb0b832b8470935a82320634b0c0951ae3a3e68179df548e290706506d8acfd3020f02c5529636e5762fad5796d798127ec5b9ce71ecd9e24e4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
