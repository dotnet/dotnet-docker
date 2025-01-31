ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:10.0.0-preview.1-alpine3.21-amd64

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25079.4

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='459d1ea33397e97cff8593e2b6e0702f8aba8ab718ed3a31e7c6aff9d99ddf69b3f62d87cdd4ece6bf001e89c59f98955384265f713f1617540a1bd13492df30' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
