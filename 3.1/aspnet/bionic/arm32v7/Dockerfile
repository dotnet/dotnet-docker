ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.0-preview3.19555.2 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='d5162decae6a916ee7a9fadf356f461eda619bab87065e1fd7c3b38f5df52e5aea4e85c84109868185831712313d99c3af5bc112c7509278f32bf03a95b7b20e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
