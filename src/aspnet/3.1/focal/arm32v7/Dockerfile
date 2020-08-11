ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-focal-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.7 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='475d5d04365602a17983d4bccbb3d6dbd7cac9db4a4c68cb6ea6398ff1769af4168d0f96c5fdfddbcbc9fa7e5adc18c078bbfcfa896f030aa8dd88d48a939923' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
