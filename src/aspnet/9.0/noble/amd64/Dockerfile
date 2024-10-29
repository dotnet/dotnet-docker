ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rtm.24514.7 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='edd4cfef374a310ee5b462e12e428dd2fec9d9691742e59663dfce1859af106d19f18819e3748d461cf51e1476c4ee0ba04f70b46491d07865a8f93d96fc2a9e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-noble-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rtm.24514.7

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
