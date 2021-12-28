FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /game
COPY bin/release/net5.0/publish/ ./
ENTRYPOINT ["dotnet", "Game.dll"]