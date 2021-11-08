ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.21-bionic

# Install ASP.NET Core
RUN aspnetcore_version=3.1.21 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='f59252166dbfe11a78373226222d6a34484b9132e24283222aea8a950a5e9657da2e4d4e9ff8cbcc2fd7c7705e13bf42a31232a6012d1e247efc718e3d8e2df1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
