## Commands
### build
```sh
dotnet build
```

### hot-reload
```sh
dotnet watch --project FosterRoster
```

### create database migration
```sh
dotnet ef migrations add {Migration Name} --project FosterRoster.Data
```

### update dev database
```sh
dotnet ef database update --project FosterRoster.Data
```

### publish to docker
```sh
dotnet publish ./FosterRoster/FosterRoster.csproj -c Release --os linux --arch x64 /t:PublishContainer
```

### check for outdated packages
```sh
dotnet list package --outdated
```

### backup database in dev container
```sh
docker compose exec -i -u 0 db pg_dump -h localhost -p 5432 -U myuser -F c -b -v -f /roster.dump mydb
docker compose cp db:/roster.dump ./
docker compose exec -i -u 0 db rm /roster.dump   
```

### restore database in dev container
```sh
docker compose cp ./roster.dump db:/roster.dump
docker compose exec -i -u 0 db pg_restore -h localhost -p 5432 -U myuser -d mydb -v /roster.dump
docker compose exec -i -u 0 db rm /roster.dump
```