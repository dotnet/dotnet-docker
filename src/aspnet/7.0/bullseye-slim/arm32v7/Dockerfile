ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/7.0.0-alpha.1.21566.6/aspnetcore-runtime-7.0.0-alpha.1.21566.6-linux-arm.tar.gz \
    && aspnetcore_sha512='02b44f090be3dccbf56f0c3dd248cc1c8efeab59ed3b0be58151e592fea9fe759416d87207a46cabe5b82c5e3be72c3c5390efb08d1ebdd5d5f71090008c949e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=7.0.0-alpha.1.21566.6 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
