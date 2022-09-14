ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:7.0.0-rc.2-cbl-mariner2.0-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-rc.2.22451.11

# Install .NET Runtime
RUN curl -fSL --output dotnet-host.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-host-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='76c9dac29a1f6cb75a7020e50c59f276ff1945601a8bc6e89c143e4e358ca1d81e26bb34f81bfa3911d528d869632ffc4278cf96ac303117b12010c04414842b' \
    && echo "$dotnet_sha512  dotnet-host.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-hostfxr.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-hostfxr-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='21a435c813c8bd92fc70eaf5bbe0d1238b37b7ff4db7132e3ef79b0d4e05845689534224bb0d1bc9913a75d18be138e3e1a94f1179c5173ce100d85b0f64a3ea' \
    && echo "$dotnet_sha512  dotnet-hostfxr.rpm" | sha512sum -c - \
    \
    && curl -fSL --output dotnet-runtime.rpm https://dotnetbuilds.azureedge.net/public/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-x64.rpm \
    && dotnet_sha512='0afe1ff93d3b362000d91472e989a0fa77535bacaae11aa3b3e149ee0d77f91a5e5f5efcea2b0b222eb50aaf47105505b767565c24fd25ea7efb7588decb6b92' \
    && echo "$dotnet_sha512  dotnet-runtime.rpm" | sha512sum -c - \
    \
    && tdnf install -y --disablerepo=* dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm \
    && rm dotnet-host.rpm dotnet-hostfxr.rpm dotnet-runtime.rpm
