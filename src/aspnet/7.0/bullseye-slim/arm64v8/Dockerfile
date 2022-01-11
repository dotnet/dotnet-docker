ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl AS installer

# Retrieve ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/7.0.0-alpha.1.22054.8/aspnetcore-runtime-7.0.0-alpha.1.22054.8-linux-arm64.tar.gz \
    && aspnetcore_sha512='6e8cd1398775117f780334474ede4acfc308c2a7e97863b3a31017747eaf0d9a51fd95eaaa209fdb0bbe93d74d850156c1c576f253100887d7e349a47ad9012b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.22054.8 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
