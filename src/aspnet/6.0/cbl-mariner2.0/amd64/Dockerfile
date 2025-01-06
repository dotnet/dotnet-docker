ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.36-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.36

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://builds-int.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='b71ad7f65043051476045bf5bffa3fc3607f37d04f61136420e71a1325fb3cadb7ad8ce5b8c174954d53411b6efe5629c6bdfec5333cdbc57131122de2c28c59' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
