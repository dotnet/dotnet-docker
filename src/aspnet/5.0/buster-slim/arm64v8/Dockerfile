ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:buster-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.16 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='624737bd29475b923ba7e3c161df4dcffb0463c1b1308442f4a88aa59940d9c943624d3bfc19ed3dad39347f093c82d269f94ce547cb59b138863d510cc2bba1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.16-buster-slim-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.16

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
