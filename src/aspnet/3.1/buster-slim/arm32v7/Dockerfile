ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.24-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.24 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='3d14f2c1af96173b0bd9734b7c833977b6d69ceae791471445023c1fad25639a2166d6b2ca7b57ce09b6c060073faafe470a9599c6d30fd25dda63f55905daea' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
