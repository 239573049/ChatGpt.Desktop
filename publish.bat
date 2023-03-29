dotnet publish "./src/ChatGpt.Desktop/ChatGpt.Desktop.csproj" -p:PublishSingleFile=true -c Release --os linux -o ./linux
dotnet publish "./src/ChatGpt.Desktop/ChatGpt.Desktop.csproj" -p:PublishSingleFile=true -c Release --os win -o ./win
dotnet publish "./src/ChatGpt.Desktop/ChatGpt.Desktop.csproj" -p:PublishSingleFile=true -c Release --os osx -o ./osx