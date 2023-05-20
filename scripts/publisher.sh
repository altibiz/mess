#!/usr/bin/env bash
set -eo pipefail

export NODE_OPTIONS="--no-warnings"
export NODE_ENV="development"

yarn publisher
