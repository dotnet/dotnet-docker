ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.6.23316.5 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='8547401ddff8b748b60df2968d0884787aea56872552743eb0750be023b8dab067f246a534c65020e945cf67ff29f7a3f55556234d615bfa67b8d77b849b15de' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.6-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23316.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
