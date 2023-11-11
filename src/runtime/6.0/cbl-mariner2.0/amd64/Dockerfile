ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.25-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.25

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4e6ad80343e4aaf5ffeb90fa172d0665ddb061ede3b05fb7ea92d439bbe36b3371947bf921434daf793a1b20e8cf95fd118dd137db1a8a6dc34e27922d6a85fc' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='b6074c0950d712df94ef08086e81dc185d9f631cb4ecd5954860df3f960705273aea28cb0edbfc99cf30a137040d382ea4ccdea307359ef44d7c67f650d5a13f' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='0abd9bb59e2bea8a0fda00d5dd27d8cebf5a6eba40dec5502f304a7dd6cfd121f0f553041ff0b48faa8b1716d26948ec9f95aa2717a4d7411bf05f1cacc51f27' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
