FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /App

COPY . ./

RUN dotnet restore 
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS run
WORKDIR /APP

COPY --from=build /App/out .

ENTRYPOINT ["dotnet", "Crawler.dll"]
