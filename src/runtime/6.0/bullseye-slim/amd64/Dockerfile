ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21415.6/dotnet-runtime-6.0.0-rc.1.21415.6-linux-x64.tar.gz \
    && dotnet_sha512='ada095fe1288afd86b7b8f2fe059f5657eab24e3060084d6006709294fa12906b648ba1406dce7f76557769b85759899342f6dc77bd62b5675cece4b3c1ffce7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21415.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
