ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21518.20/aspnetcore-runtime-6.0.0-rtm.21518.20-linux-x64.tar.gz \
    && aspnetcore_sha512='61c1feb088ab0d9024b3aaf00b30c7764e172ffb64f1361b02ec8d6ae34ec9eed10d3be8708f0c73e9ce2439329e68417e0716eb72ffac0f8c63f96bccc26818' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-bullseye-slim-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21518.20 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
