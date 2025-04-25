ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:trixie-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25224.106 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='e12fba0c9060c2b35edc073c0d4ad4248833606a4cd49784acc98ff7a98b1a02a0c5dfa0507fae2dc2d4b444a84e652b75deec49e18385898607e1549e417b62' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-trixie-slim-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25224.106

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
