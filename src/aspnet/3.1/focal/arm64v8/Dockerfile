ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.20-focal-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.20 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='77888dcac7d92793674a861a6b9a7fc0a8de597d0d698d7dd1fac5ed0293020d8200cd441f64994f1121d7ea3b582551a799f8664775e26105dd9b91d95a1c4b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
