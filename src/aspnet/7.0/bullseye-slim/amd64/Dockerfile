ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:bullseye-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-alpha.1.22063.25/aspnetcore-runtime-7.0.0-alpha.1.22063.25-linux-x64.tar.gz \
    && aspnetcore_sha512='2e4e4237068815dadcfa75c6c2e2d151d42a81b48ce84f8f8df69e79cc69489d9a4a065d119d88e1f61de73be3f5e8d973bf216131b128913e463173dd528fbd' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22063.25 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
