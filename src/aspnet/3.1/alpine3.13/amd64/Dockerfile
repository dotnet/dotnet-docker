ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.17-alpine3.13

# Install ASP.NET Core
RUN aspnetcore_version=3.1.17 \
    && wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='8861552d21a7efed982f31d228d42da7f8de8cb5343771de96034b3ddcabca2f9bc96bdadc40c7732c217bbc1b4d68999e91a98541076d6b5f535c2e6c9f8a4f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
