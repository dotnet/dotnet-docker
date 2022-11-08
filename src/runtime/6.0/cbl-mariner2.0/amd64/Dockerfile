ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.11-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.11

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='75901f40eb9ae528d8cf7ae20a14ba092e05d1eccac0b0df87492553fb96c8fe67fe7eed902286324b4ee0a4f68c8fc541a859f3a0a4fbc72bd02cc1ad7d90ce' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='4e289c27a94bd0571105bd4b6433031c8cc8d5cca191089906e350222b86ab38960f4c39c59e8a6b0227fe6194d0bb2ab45c969a9688e60bdfbb132c39a855b2' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='12950dfaacc42cc187b0610176a4247dc49cb8b4f333978a00a8f4a70e3e1cf0af1142d1efc8df9d4a089c488529bfa22c37d6bdfb953b609cba7b6e7a7e3170' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
