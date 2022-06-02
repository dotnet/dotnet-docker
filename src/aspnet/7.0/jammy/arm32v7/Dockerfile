ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-preview.5.22273.7 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='12158a31d3c537cffa38216ff9728d84ec7391f7eaa980ca3390091264a08c2940e5cc3838f690e664862275ea4237c03ab69a2a8f3a3317270550aedc97755b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.5-jammy-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22273.7

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
