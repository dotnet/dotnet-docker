ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-focal-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.13 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='0480835c6daff68e270b13f07ea5c6c3620f78c9b4f7b0448e842887d11014b53107692aacaecaa57a684814ad744a3af675f7bbc2662c7f40024f905c19db08' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
