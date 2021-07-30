ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.8-cbl-mariner1.0-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.8

RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='a3e89a32bf1683ea9655a7fc1a2e638ed34e3c0fd3993b5e3356a3bb089514c9a63de134639393cbe54bde9b77207b8ebcea0898418770310e7c485d946b3f34' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
