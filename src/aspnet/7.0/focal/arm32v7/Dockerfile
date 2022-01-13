ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-alpha.1.22062.12/aspnetcore-runtime-7.0.0-alpha.1.22062.12-linux-arm.tar.gz \
    && aspnetcore_sha512='1445012951ce9775ef7060d39b5a691d87466f3a927cad195b618b82505b022a2cbb79b1472eedaf5dcc2f20f14a5ee195459f1325c97a5895a7239220100f9a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-focal-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22062.12 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
