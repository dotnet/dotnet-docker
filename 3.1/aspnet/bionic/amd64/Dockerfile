ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview3.19555.2

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='4005ceaecf160821d1f2df76c30661bd939e3a4ab0e73b5c7d557db8d1559b48f7afe4c1e7e4ca86ea47191d003865dc272a7acf761643fa060dfcbc868ba25c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
