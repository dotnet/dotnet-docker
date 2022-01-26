ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-preview.2.22074.8/aspnetcore-runtime-7.0.0-preview.2.22074.8-linux-x64.tar.gz \
    && aspnetcore_sha512='cbd62ed9ab7a5a867c822a1553752248bb6b3f53df4dbbe0160f1c1303005bc65e299c73c019245a9fdf0b2a9d1c5c1393926c8d0457f117c5cbd5f623ed2f18' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-preview.2-focal-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-preview.2.22074.8 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
