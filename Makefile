##Baseado em https://github.com/AlexSugak/dotnet-core-tdd/blob/master/Makefile
setup-ubuntu: 
	xdg-open http://localhost:5000/howto & \
	docker-compose up -d  && \ 
	

setup-windows: 
	explorer "http://localhost:5000/howto" & \
	docker-compose up -d 
	
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