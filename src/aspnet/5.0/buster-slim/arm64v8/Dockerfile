ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.10/aspnetcore-runtime-5.0.10-linux-arm64.tar.gz \
    && aspnetcore_sha512='e86dd5b26e5add2f35c3a029c78e3c447755a319c105d371da297f66db5eff260f3f55ccf23e508e9a33536876708ac2e358dc62a4a28518f88df3a9131adb01' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.10-buster-slim-arm64v8

ENV ASPNET_VERSION=5.0.10

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
