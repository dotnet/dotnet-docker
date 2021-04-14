ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.14 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='86507136494d16e83833c2fb0bed6afdf4b901c37c5057c94b3c35cf803cbf5a00d00ff54bbd2dea9b6bb81ec0860081cd911f61dd10f918de89bf697a1e7baa' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
