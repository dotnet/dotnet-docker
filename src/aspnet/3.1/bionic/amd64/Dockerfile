ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-bionic

# Install ASP.NET Core
RUN aspnetcore_version=3.1.10 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='884ec943eefc8397537a193d48d481eae8869eb82a8149f11b8a8bbca0cd75307e82e4db04a2329f03f8a50519afa27c0caa79193fb35a9c776efe1aff2d07a0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
