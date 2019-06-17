ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.9

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19315.2

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='cae7edbe2509f00cdf8012ce0499dc607e7f32f579ca58a77c04905fe613fb1406d8e4775d7f8fad2e594ac6b42ebb8c9ca933afcf525d707262bf2ab7aff9f3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
