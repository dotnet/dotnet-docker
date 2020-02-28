ARG REPO=mcr.microsoft.com/dotnet/core/runtime

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.1.20124.5 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='bc4d66c2c81ba81afa63226444bad97d590b08c35a4f13e2bc972fd11f313bc524286384925c1db0d882acd5ef17ea9cc7715331cfaa2394019cd5bca461d6f5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-buster-slim-arm64v8

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
