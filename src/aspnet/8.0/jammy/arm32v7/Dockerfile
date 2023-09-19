ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-rc.2.23468.15 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='54c0a9abf61550b4f637b889437475e7f54272e9025317a3fb1868be4bd4e2132fc2e0a6812cf2343d04275788f2300ba4a9ed8f9ae0be95e9fd1074fe2c2390' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-rc.2-jammy-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-rc.2.23468.15

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
