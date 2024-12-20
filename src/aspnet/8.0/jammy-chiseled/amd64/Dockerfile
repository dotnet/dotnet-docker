ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.11 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='e7acf9dc5cfa49aa7ec30dbb9586bc7beaac9e3116c75303b511770e3597b209739f28c754b2107c0255acac90187cd1000c1ee772463fc828934a4dda35f5c3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.11-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
