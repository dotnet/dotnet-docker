ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:buster-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.15 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='e0d0f5150bac6234dda07da91983eb1a4cb13320d9e06fe83dfe4155abbb4125ce8053638aced998cd9d040a5a595c97773004d16123a9bff6da43a9c0e266eb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.15-buster-slim-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.15

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
