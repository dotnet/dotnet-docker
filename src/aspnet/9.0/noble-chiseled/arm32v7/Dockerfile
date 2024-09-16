ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.2.24463.6 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='ec58ecedcff54fb8149f296cecc2cb591f7e9f4c05758993ff60db88bcc5be864aeac2d53cdf82e3fab8cfc8f234564a3c51a98a874b39d8c2b6b85eb3c45937' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.2-noble-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.2.24463.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
