ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.17-focal-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.17 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='7fe6443deea56bdef7c990560113b86a2f40862eb3fdbe1f5307ac461aa563d6106c9cf6d8caee08de375e82e598571e742460832c36a3c669e44a0522d5af87' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
