ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.36-alpine3.20-arm64v8

# .NET globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.36

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://builds-int.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='cf3dbc687acd27220560baf52d52dd42f4773ddd642487ae84e7a298ac30d9640af86cd0c5ee28ecd0364b35892194dbdfd1df302451367d7472c3bd00202f26' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
