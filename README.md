# PktmonService
Windows Background Service to Capture Network Traffic using Pktmon


#### Create new service
```dotnet new worker -n PktmonService```

#### Copy and replace
- Worker.cs
- Program.cs
- PktmonService.csproj

#### Publish the project
```dotnet publish -c Release -r win-x64 --self-contained```
- Refer to https://learn.microsoft.com/en-us/dotnet/core/deploying/ for publishing types

#### Create new service
```sc create PktmonService binpath= "path/to/service/PktmonService.exe" start= auto ```

#### Start / Query / Stop service
```sc start PktmonService```

```sc query PktmonService ```

```sc stop PktmonService ```

