ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.28 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='6a6787f692f27fd8e437787cbc69840c7e19723db5d155802b2ed86917d6e8ca7d114782839a167dee0b23c3af09c9d82f938aa7fbb6c66a10bd510e577e40b5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.28-jammy-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.28

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
