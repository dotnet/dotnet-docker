ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.11

# Install ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.2.20155.1 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='2c5bd4d2c9cad88b87b183d9756b471fc421d2c0dbb7a8e4cdebcca71af452a9565e1f88a7d4bf622e504aa1dd209f4362015616b31ebbf111d2fc7c83824caa' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
