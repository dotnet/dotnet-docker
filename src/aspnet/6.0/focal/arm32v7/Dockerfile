ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21518.18/aspnetcore-runtime-6.0.0-rtm.21518.18-linux-arm.tar.gz \
    && aspnetcore_sha512='989948092e11570540dbb7260d4a71221c1288f8c219ecc72dc44d0e2dd646f2acc024964fe0eb1d760a4e5f8a788e9fb370e0e659b256c805c497c16cb8c995' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-focal-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21518.18 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
