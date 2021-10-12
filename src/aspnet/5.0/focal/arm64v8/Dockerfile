ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.11/aspnetcore-runtime-5.0.11-linux-arm64.tar.gz \
    && aspnetcore_sha512='70844c1c1ca12dbdef3c459ce9a27d5ac91c893891473efb7b2184596210008674078c9d9833d6a2d5114b680a182a3e83d135b3d175bb373b439fcf22f4fc02' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.11-focal-arm64v8

ENV ASPNET_VERSION=5.0.11

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
