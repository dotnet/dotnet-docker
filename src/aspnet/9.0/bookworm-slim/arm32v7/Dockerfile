ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.2.24474.3 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='d6aaa61df66bc42296350f56a13e4f5a5b56770e62cdf4bb2a647f80db3bca632e7f8b64dbb2d2b8426e862edf3ca75bebcfe9db5f6a6e94ec08557a4f7a461b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.2-bookworm-slim-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.2.24474.3

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
