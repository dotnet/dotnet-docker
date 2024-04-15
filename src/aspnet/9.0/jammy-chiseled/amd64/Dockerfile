ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.4.24213.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='5975bf6dab0a91a22eb332330d05fabad55898af6d14e8088bb8f5960554ccf1487a8978b1683c0d9aa22846bbde3bd4926ff5fb1a8a2062caaeaf7d51f94592' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.4-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.4.24213.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
