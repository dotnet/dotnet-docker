ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.31-buster-slim

# Install ASP.NET Core
RUN aspnetcore_version=3.1.31 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='9ea1fb4c9a656de8392b8f92c608c2f927fd03ad8e8b195f3f0b69c1433cfbf2679827b1ce2fac783f8ef77307c7b1b36563d0813f914b75b025b5cca6c773f9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
