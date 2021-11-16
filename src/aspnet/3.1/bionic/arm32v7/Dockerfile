ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.21-bionic-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.21 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='3f7e1839946c65c437a8b55f1f66b15f8faa729abd19874cb2507c10fb5ae6a572c7d4943141b8a450ee74082c3719d4f146c79f2fabf48716ff28be2720effa' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
