#!/usr/bin/env sh

set -e

dotnet publish -o ./bin/wms-api -r linux-musl-x64 --self-contained false

scp -r -p 2201 ./bin/wms-api root@139.198.191.120:/app
