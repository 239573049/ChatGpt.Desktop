dotnet publish "./src/ChatGpt.Desktop/ChatGpt.Desktop.csproj" -p:PublishSingleFile=true -c Release --os linux -o ./linux
dotnet publish "./src/ChatGpt.Desktop/ChatGpt.Desktop.csproj" -p:PublishSingleFile=true -c Release --os win -o ./win
dotnet publish "./src/ChatGpt.Desktop/ChatGpt.Desktop.csproj" -p:PublishSingleFile=true -c Release --os osx -o ./osx
dotnet publish "./src/ChatGpt.Desktop/ChatGpt.Desktop.csproj" -p:PublishSingleFile=true --self-contained -c Release --os linux -o ./linux-self-contained
dotnet publish "./src/ChatGpt.Desktop/ChatGpt.Desktop.csproj" -p:PublishSingleFile=true --self-contained  -c Release --os win -o ./win-self-contained
dotnet publish "./src/ChatGpt.Desktop/ChatGpt.Desktop.csproj" -p:PublishSingleFile=true --self-contained  -c Release --os osx -o ./osx-self-contained
docker compose build

docker compose push 
