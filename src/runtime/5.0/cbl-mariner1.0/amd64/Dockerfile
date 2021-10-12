ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:5.0.11-cbl-mariner1.0-amd64

# Install .NET
ENV DOTNET_VERSION=5.0.11

RUN curl -SL --output dotnet-host.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='017ca02908cbfa44e58e3c4b2c3cc6fa21212d002a9bc361dd4a27cd615af25155557627ded21b7149feeb8918df67f74f51f5e99cffd8db43a99c52cc28ada2' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-hostfxr.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='2cb42f063274ca2cf4a8c4ca21f70d684fcd8021456fba599203002d15ca89c21f7d664e85b24995a723a5ce300be6a208baa1cece9206c10e0166b565796457' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -SL --output dotnet-runtime.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='9d401b5874c2fd9f1911614669489c5aa40dfe3e83d3ca80b405993e846290326c4f68ebeb7ff0c2b7a50abf1dfa8f3434b810d81ee4aadd849d8106b7ab2321' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && rpm --install dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
