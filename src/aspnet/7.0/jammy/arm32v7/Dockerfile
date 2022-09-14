ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-rc.1.22427.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='1dff328a00c25fb7dbf3ed267afbce8aa76c8b3077015b27028476fe3f7a77ed727db971bb6de2a9070e647b2969e1992505086c369b9c95eafebc8bde20a8e9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-rc.1-jammy-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-rc.1.22427.2

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
