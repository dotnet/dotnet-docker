ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-alpha.1.22054.8/aspnetcore-runtime-7.0.0-alpha.1.22054.8-linux-x64.tar.gz \
    && aspnetcore_sha512='5d70059032ce0e867d511ac86bd9e5b18e47551c7e134b12b7d5ef8116aa3ec49a742256412149095479394bee050e13970d30e5648e8aaf7e0f99be20844386' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-focal-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22054.8 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
