ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.3 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='9e7044391ea1062dd3213509a59bfeff101238eb53d7bc2789079ec88bc30e7cace75fac570e92da9c843b13e27a878eaeb8ab4ea9e630620ca96c8dab18fdb7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
