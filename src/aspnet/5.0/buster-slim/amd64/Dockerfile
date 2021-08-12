ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.9/aspnetcore-runtime-5.0.9-linux-x64.tar.gz \
    && aspnetcore_sha512='67ac99b8f631684961aa2e887ed5d6b9cc301b06a1842ac212791860bf6de145831228df5f57eca8a97e16e569e7b5a308fd20d82f11569c375c689fc932dfff' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.9-buster-slim-amd64

ENV ASPNET_VERSION=5.0.9

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
