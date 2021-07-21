ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-preview.7.21370.6/aspnetcore-runtime-6.0.0-preview.7.21370.6-linux-x64.tar.gz \
    && aspnetcore_sha512='067237375d7ace10da8349ee4c6a17d828844693764a125159589f76cc2d15eb9112a5c4b64cb14e8b453defbd420300bdf0293df0e441cfd4f32ade9719d895' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-preview.7-bullseye-slim-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.7.21370.6 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
