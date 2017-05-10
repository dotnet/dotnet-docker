FROM debian:jessie

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
# .NET Core dependencies
        libc6 \
        libcurl3 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu52 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        libunwind8 \
        libuuid1 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Update libcurl to version 7.53.1
RUN echo deb-src ftp://ftp.de.debian.org/debian/ stable main contrib >> /etc/apt/sources.list && \
    apt-get update && \
    apt-get -y build-dep curl && \
    apt-get -y install wget

RUN mkdir ~/curl && \ 
    cd ~/curl && \ 
    wget http://curl.haxx.se/download/curl-7.53.1.tar.bz2 && \ 
    tar -xvjf curl-7.53.1.tar.bz2 && \ 
    cd curl-7.53.1 && \ 
    ./configure && \ 
    make && \ 
    make install && \ 
    ldconfig && \
    cd .. && \
    rm -rf curl