ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-preview.6.21355.2/aspnetcore-runtime-6.0.0-preview.6.21355.2-linux-x64.tar.gz \
    && aspnetcore_sha512='42cc384554caef8f72b5410aab196f3d62cdb053dbc56e8bd4e0411de5deac19b9524b7cfebb14c69d583842b477e830bc34e8e9d76f723ea02470488d172f47' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-preview.6-focal-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.6.21355.2 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
