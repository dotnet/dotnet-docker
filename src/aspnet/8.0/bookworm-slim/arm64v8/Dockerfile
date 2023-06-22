ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.6.23320.14 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='5423bfed5935b854f656c5a9aed5335844eec3907595ffa8756916d72535be19c6304f61c43813e660fd60ad5bb8393ebcb09f9e8a27b0bff8002a6fbd3aa655' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.6-bookworm-slim-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23320.14

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
