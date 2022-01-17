ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-alpha.1.22066.1/aspnetcore-runtime-7.0.0-alpha.1.22066.1-linux-arm64.tar.gz \
    && aspnetcore_sha512='33165e42367fb25dc7fab58491999ab4df668b98263ec95bb8e2d813071b659e876a7e478126d1d1a829a541f9de5ec7bdc6e869c36b25fbc50686e3ce110d62' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22066.1 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
