ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:buster-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.15 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='15185d0a58e69db258ace42022076184815c8838ba195b405bdc3b4187e311f2d0f2f6876f752d5ba16c5e87ddb75299bfbf5a438d27df02d1cfe02463cbf5d7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.15-buster-slim-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.15

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
