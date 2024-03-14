ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:6.0.28-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.28

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='cb99220b3bb92bb691b4f6b69bad90d5413bc50911ea176fa8623fb320f08ad66fbc6c194d8cfea007565f976dce1b1d2588e55dcdac49c6db6a0fd3a0b7da5e' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='fca21061267a476cd980d7c5039cbb91f8af1cc534b36cd0365ad76b6e881f283221d3a859c93668c7ff56367b692c95dc5bc5ce3087fdc606f1ebcd5dbfcc65' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='a33ba0a5e9feab789284089f7b38dd6c9d1b88a45bbaeb93d56249781c9b7bea1cc382cba21a1af848386d39d2dec2a0a29fd8df2e3a029f7017e11cee2c3382' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
