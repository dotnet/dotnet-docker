ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-disco

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview5-19223-06

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='758c4b33892a9b882df2ad14039b53c08139a5d40ff088ea64a382b7ee9028aaf1a224410094ab1bb2a29e7cb5898f8fd07097ac96f23566e36b175efb20854f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
