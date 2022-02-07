ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.14 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='a7f2e3525226d87ecd52c7ee0e702523cd148c7d619d4d3ffab3d4b14bcfa94909016008791aee9b2d6c84adc25d22acefa2a1a6fb7ac60ef0951ff4aa018a66' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.14-bullseye-slim-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.14

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
