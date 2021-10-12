ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.11/aspnetcore-runtime-5.0.11-linux-arm.tar.gz \
    && aspnetcore_sha512='3464ff4341e9e8642f3a204039974e5b959e4149c00137fa64452e9540a8132520e2d8ff19dab5fbdff40d22fbb94c006242d7602022fe94aaa5216cd4e3e438' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.11-bullseye-slim-arm32v7

ENV ASPNET_VERSION=5.0.11

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
