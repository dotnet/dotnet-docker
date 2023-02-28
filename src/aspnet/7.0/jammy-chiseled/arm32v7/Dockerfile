ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.3 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='173dbedc3a01b5d6fd70f617935a5d65022fbd6d16d3dc0e74959b1197c90d4e790b44e3f93a1d7f440d3c89568000d7ee048dfc414a6eb570da7639cfb20b51' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.3-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
