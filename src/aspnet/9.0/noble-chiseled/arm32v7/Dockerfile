ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.7.24373.9 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='945264ecf93107cc8eb21934277bc8ee4bc7af84636714d5d04b0b59b923ade8dee420b8c5bd5f8f33025d073824157e2d4f6e13f1a59bef62e8302fdb3f0a15' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.7-noble-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.7.24373.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
