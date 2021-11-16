ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/7.0.0-alpha.1.21551.1/dotnet-runtime-7.0.0-alpha.1.21551.1-linux-arm.tar.gz \
    && dotnet_sha512='221415375c48f296139e00238c910e0bb87e314e2348cdaf7286614ce89faa4f572ce3fe0272ed89def45c18f8d14c3b5270b5ac8ede1fa7b9a75a663435c450' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-bullseye-slim-arm32v7

ENV DOTNET_VERSION=7.0.0-alpha.1.21551.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
