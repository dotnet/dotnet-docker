ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.30 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='757d017db28b8e34c4b242c082aa51eb85bce8fca16af37a0beabedb271c9bd13e1631868faa131745d7784db48974567f82275da09041ba434dcb7abe821a2d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.30-jammy-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.30

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
