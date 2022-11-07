ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.31-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.31 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='970def9298bfe39c00054ae45231e2c632d4364a311349b3594bef5dd3739af2db33329f314f29a3956c271745948df88076e39bd2fa80e8a4dbb9723e3493ec' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
