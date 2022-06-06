ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.5-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.5.22301.14

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='95792f8f3189343aa1523bff946e65d89222e4ea6e26be3103ef38d23fa275f0a44eb50d8a615ebde05cf4552cb8ddd02beb2a65308d0a0b4f99d49612a06f46' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y aspnetcore.rpm \
    && rm aspnetcore.rpm
