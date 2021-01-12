ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-bionic-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.11 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='30d302fa7d14d73091cc102f147a185a8fea232afde0576af88f56af282201d397aae5a2a50d2b4ab4be9efb165729a80cbfedc61490596324625d13297806fc' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
