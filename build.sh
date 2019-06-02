#!/usr/bin/env bash

set -eu
set -o pipefail
#use latest dotnet sdk from snap
export DOTNET_ROOT=$HOME/dotnet
export PATH=$PATH:${DOTNET_ROOT}
#enforce rolling to latest dotnet version
export DOTNET_ROLL_FORWARD_ON_NO_CANDIDATE_FX=2 
export DOTNET_ROLL_FORWARD=Major

# liberated from https://stackoverflow.com/a/18443300/433393
realpath() {
  OURPWD=$PWD
  cd "$(dirname "$1")"
  LINK=$(readlink "$(basename "$1")")
  while [ "$LINK" ]; do
    cd "$(dirname "$LINK")"
    LINK=$(readlink "$(basename "$1")")
  done
  REALPATH="$PWD/$(basename "$1")"
  cd "$OURPWD"
  echo "$REALPATH"
}

TOOL_PATH=$(realpath .fake)
FAKE="$TOOL_PATH"/fake

if ! [ -e "$FAKE" ]
then
  dotnet tool install fake-cli --tool-path $TOOL_PATH
fi
"$FAKE" run build.fsx "$@"