##Baseado em https://github.com/AlexSugak/dotnet-core-tdd/blob/master/Makefile

build:
	dotnet build src/ApiShortUrl.csproj


restore:
	dotnet restore src/ApiShortUrl.sln

package:
	dotnet add src/ApiShortUrl.csproj package $(ARGS)

rebuild-docker: 
	docker-compose down
	docker-compose build --no-cache
	docker-compose up -d

restart-docker: 
	docker-compose down
	docker-compose up -d