& {
    $env:DOTNET_WATCH_SUPRESS_EMOJIS="1";
    $env:DOTNET_WATCH_RESTART_ON_RUDE_EDIT="1";
    dotnet watch run --project Watcher.csproj
}
