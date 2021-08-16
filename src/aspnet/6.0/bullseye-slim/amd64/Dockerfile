ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rc.1.21413.12/aspnetcore-runtime-6.0.0-rc.1.21413.12-linux-x64.tar.gz \
    && aspnetcore_sha512='deed736d75bf18c335201af5dcb12a63fcff9b876ff535cf3961a06454e784dd56cfbc7ef8b14bed9498a73ed729b15cb9b84d76658132afdbf374ed64491c1c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-rc.1-bullseye-slim-amd64

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rc.1.21413.12 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
