ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.13 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='72bb5a366fda26eb820c181dc76e43d3fa040feedcc76dadc9064b924c98e2c31c4d4f53e6cac5c325bbd3640b71adde2c605c4cb488435515049ffe9221864c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.13-focal-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.13

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
