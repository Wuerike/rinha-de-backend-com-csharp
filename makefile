-include .env
export

run:
	dotnet watch run --project src/RinhaBackend.csproj

up:
	docker compose down
	docker compose build
	docker compose up