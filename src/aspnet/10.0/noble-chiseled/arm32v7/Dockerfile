ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.1.25120.3 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='253c3e18705954121c843d367e3b2cbb8ffc46649f2e0db255b35833163e5dec6b5df41c4970102281af68cf8d912222db168de39e2171af39b0cc163bc17eb7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.1-noble-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25120.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
