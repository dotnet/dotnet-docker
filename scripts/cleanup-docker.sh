#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

docker stop $(docker ps -q) || true
docker system prune -a -f
docker volume rm $(docker volume ls -q) || true
