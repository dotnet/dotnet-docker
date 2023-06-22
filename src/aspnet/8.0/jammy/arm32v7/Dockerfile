ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.6.23320.14 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='5fd77a91b9ebaa3c6f0aaf66b774a2b3e6e66dacd99b6c4afdf4a42e209e699d99fecb2d0077f83208009473b0b9688f9bbd45f88c2468d2ec207f1e65332f81' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.6-jammy-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23320.14

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
