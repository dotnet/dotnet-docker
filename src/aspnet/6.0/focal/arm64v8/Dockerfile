ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.2.21480.10/aspnetcore-runtime-6.0.0-rc.2.21480.10-linux-arm64.tar.gz \
    && aspnetcore_sha512='3a92e93a8cb0c186f1c4a822d46a37604bc470599dbcbe05a7f1ea7c7d1dd9c0e6571de524de8729d19d237078742a0b4e3a1daa11b5eb5f8cce74b69710ada4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-rc.2-focal-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.2.21480.10 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
