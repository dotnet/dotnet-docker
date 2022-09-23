ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-rc.2-cbl-mariner2.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-rc.2.22464.13

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='ceda4e6f8866016eec26ad0763f6a4e8f900dcc0af04219c244b5b29f2f228b06cbabd8d7e25ed1b2a01326c5dcaddd428b79a65e27792abb3ea3555ec4f8677' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && tdnf install -y --disablerepo=* aspnetcore.rpm \
    && rm aspnetcore.rpm
