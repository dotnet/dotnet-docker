ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21513.16/aspnetcore-runtime-6.0.0-rtm.21513.16-linux-arm64.tar.gz \
    && aspnetcore_sha512='d222acafa4417ea95a074069768e753e4b29e4ff0c6f4f5fa109fd03b4605dab6de9ee602d64c813ea4d53b10a24ba4c94be4e2a73df61a8d20f01e5225c451e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-focal-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21513.16 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
