ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-alpine3.13

# Install ASP.NET Core
RUN aspnetcore_version=3.1.16 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='0357a441f085cd91d61cefe02b3933a77fbac9dc2b0b4b7dec4dccf13220ce7b25152f2f17d6d6a14fd3762cac40f0a6cdcf5d56dd44b4724762ea2caeba6c87' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
