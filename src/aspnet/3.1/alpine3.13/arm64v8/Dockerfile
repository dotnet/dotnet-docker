ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.21-alpine3.13-arm64v8

# .NET Core globalization APIs will use invariant mode by default because DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true is set
# by the base runtime-deps image. See https://aka.ms/dotnet-globalization-alpine-containers for more information.

# Install ASP.NET Core
RUN aspnetcore_version=3.1.21 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='6d3b66ddfe93fbeea9b85d6efdb253c826f41339d843195a7ba2c4ef6604a4de9890ea84305d70ac17ec444f4344947c792abc7ce38286e2c26f5b16c8fcd7a9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
