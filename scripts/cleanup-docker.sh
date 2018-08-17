#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

docker stop $(docker ps -a -q)
docker system prune -a -f --volumes
