ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.25-bullseye-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.25 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='fc6ea45a5b89f7ff66a863e89b477d94b329a3f6e0b03b9dfacc31a85a0b915f4f68aade825bbef1e0bb0f4b6fe7c173be7cb24b5cef685217d6a3919598d465' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
