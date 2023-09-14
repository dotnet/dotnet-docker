ARG TAG=8.0
# ARG TAG=8.0-jammy
# ARG TAG=8.0-alpine
FROM mcr.microsoft.com/dotnet/sdk:$TAG AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore --ucr

# copy and publish app and libraries
COPY . .
RUN dotnet publish --ucr --self-contained false --no-restore -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:$TAG
# For Ubuntu
# RUN apt update && DEBIAN_FRONTEND=noninteractive && apt install -y tzdata
# For Alpine
# ENV \
#     DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
#     LC_ALL=en_US.UTF-8 \
#     LANG=en_US.UTF-8
# RUN apk add --no-cache \
#     icu-data-full \
#     icu-libs \
#     tzdata
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./globalapp"]
