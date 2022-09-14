ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-rc.1.22427.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='101898d0921f21c7895a1e015064a5aa5f1ca92bd31bd0d30fb9981e3c4383f14ea25464289e4ef29bf55fea1e2096e4b07bce71c948992a76c5ff0f7005b415' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-rc.1-jammy-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-rc.1.22427.2

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
