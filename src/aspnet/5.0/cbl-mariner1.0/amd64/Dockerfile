ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.10-cbl-mariner1.0-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=5.0.10

RUN curl -SL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && dotnet_sha512='01e581edc981e5ea169107bce76249bbfb2139c69ae1627ba705c486d44082f4e9cc7cc0ac6362c9c49860e19eb2894b28865e5ef49a2b8eb66ed001cf35cc53' \
    && echo "$dotnet_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
