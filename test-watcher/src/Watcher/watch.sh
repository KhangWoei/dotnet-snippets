#!/usr/bin/env bash
DOTNET_WATCH_SUPRESS_EMOJIS=1 \
DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1 \
dotnet watch run --project Watcher.csproj
