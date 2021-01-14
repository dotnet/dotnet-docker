ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-buster-slim

# Install ASP.NET Core
RUN aspnetcore_version=3.1.11 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='626cbf5f4c76c9382403f18cc288a925e62d6bf821249be5c0ef2f58278479dd95ee4bc243377554a835eda0c2409c5d8e529c5f9712ae7475ce0420c8df4b72' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
