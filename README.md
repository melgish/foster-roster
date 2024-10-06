## Commands
### build
```sh
dotnet build
```

### hot-reload
```sh
dotnet watch --project FosterRoster
```

### update database
```sh
dotnet ef database upgrade --project FosterRoster.Data
```

### create database migration
```sh
dotnet ef migrations add {Migration Name} --project FosterRoster.Data
```

### publish to docker
```sh
dotnet publish -c Release --os linux --arch x64 /t:PublishContainer
```