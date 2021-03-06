#!/usr/bin/env sh

set -e

cd ../Tiantong.Iot.Client

yarn build

cd ../Tiantong.Iot.Api

rm -r ./release || true

dotnet publish -o ./release -r win-x64 --self-contained false
