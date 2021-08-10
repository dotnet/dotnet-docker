ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.18-focal-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.18 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='373d8314f267c9c49df866e14eb98b0600a7e2a68157f84b0c08762417bd8ebace6b2acf214c5749ad1c1b00989b9e04bfa9f1d802269c12b8bd2ff8434d7f86' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
