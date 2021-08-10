ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.9-cbl-mariner1.0-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.9

RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='fda4073ebc35b2cd9d36dc4a9152b55955fb45b8ede03ec3396c6b7444114ffb6b2c6bfca1cb72026f58a9ff82f20ed631ee23ffcb75d2c912ce1ad3e84083e6' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
