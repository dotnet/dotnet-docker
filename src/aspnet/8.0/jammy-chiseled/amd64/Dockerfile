ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.4.23260.4 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='fa2f95d90dafc11e083904e5795a0f9011ccd135a94e64149795a4404a900f0962d2d8cb187db2e5d32e0d8ca55387626e7201ac0c8127fba0cb499eb5a78c61' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.4-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.4.23260.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
