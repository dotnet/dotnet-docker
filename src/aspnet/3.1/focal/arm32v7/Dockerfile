ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-focal-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.10 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='02e304af66734fa14042e59b0c47a1c19ffcacef6dd58f293352dd32a79b5abce303010101384fe25d20cb6391df4bdffc8e3cf766f88781a8e3b1f6b1e2ee0d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
