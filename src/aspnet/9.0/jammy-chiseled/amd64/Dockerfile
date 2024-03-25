ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.4.24173.3 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='32c8f993a5662909ca6387c4ad62965a60678ebb442136d64758649007efeed39e7b6b2da867f8534ff26f9c9b9832ceb43c512492421c38ec9b3e703bcc026c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.4-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24173.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
