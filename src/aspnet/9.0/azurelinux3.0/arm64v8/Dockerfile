ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.0-azurelinux3.0-arm64v8 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rtm.24516.19 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='412a91fd5529843a0405e16e4384894246e9d8b7119d76722fc0e7e1e4eabaae88dc94b1b3a0b501d82742ce4f9b1856c6a5b1e5864103403c535e27153b30ab' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-azurelinux3.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rtm.24516.19

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
