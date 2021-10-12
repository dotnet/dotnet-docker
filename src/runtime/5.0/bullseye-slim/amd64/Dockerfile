ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.11/dotnet-runtime-5.0.11-linux-x64.tar.gz \
    && dotnet_sha512='4eef891385288e5ab35beea79e2e6ca13615afe2e90da7a7a14417119bdbeb42980dda5f1cbe5b6d8e05a728d0db4256abb54cd00a63aa621a70090bddbe9f69' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.11-bullseye-slim-amd64

ENV DOTNET_VERSION=5.0.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
