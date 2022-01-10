ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22053.15/dotnet-runtime-7.0.0-alpha.1.22053.15-linux-arm64.tar.gz \
    && dotnet_sha512='213866d0dfc6bb528c7a532b1e435548955475a87535191426bd158d0c2536f59e3aed73fe855d1818dfb11596dd5f8818926ab1471577c3fcd996bd788bad95' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-focal-arm64v8

ENV DOTNET_VERSION=7.0.0-alpha.1.22053.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
