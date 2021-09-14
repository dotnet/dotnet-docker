ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.19-focal

# Install ASP.NET Core
RUN aspnetcore_version=3.1.19 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='c9234b5dba62c88b5f7e869c7dc7fc94d9cd7b300b0ed2461132c32762c0d3d1f23a1ee0432264704fefeb35f77ca7d1dd41910daee84df9aa325559a43cfc71' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
