ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-preview.7.21369.12/aspnetcore-runtime-6.0.0-preview.7.21369.12-linux-arm.tar.gz \
    && aspnetcore_sha512='31371207040c09df87dff64009f1df6b9e66fe5e90d24c34b48d6536cad025a84f2f948c7f9b16a66fd434025a020f968a09fbf48beb88aa30e44cb1246aa448' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-preview.7-bullseye-slim-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.7.21369.12 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
