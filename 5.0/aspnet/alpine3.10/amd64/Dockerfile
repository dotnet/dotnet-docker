ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:5.0-alpine3.10

# Install ASP.NET Core
RUN aspnetcore_version=5.0.0-alpha.1.20071.6 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='7b0cc3f1e5b15f13f97d0a47ff27a8b20a1f397caa15c8e5f4694d52f88a9ba5335e3207fd3b9ec90255bb08ae331d2d081c05c2cda7f93039e53489e53b947c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
