FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN apt-get update && \
    apt-get install -y python3.10 python3-pip
COPY ["src/ChatGPT.WebAssembly/ChatGPT.WebAssembly.csproj", "src/ChatGPT.WebAssembly/"]
COPY ["src/ChatGpt.Shared/ChatGpt.Shared.csproj", "src/ChatGpt.Shared/"]
RUN dotnet workload restore "/src/src/ChatGPT.WebAssembly/ChatGPT.WebAssembly.csproj"
RUN dotnet restore "src/ChatGPT.WebAssembly/ChatGPT.WebAssembly.csproj"
COPY . .
WORKDIR "/src/src/ChatGPT.WebAssembly"
RUN dotnet workload restore "/src/src/ChatGPT.WebAssembly/ChatGPT.WebAssembly.csproj"
RUN  dotnet publish "/src/src/ChatGPT.WebAssembly/ChatGPT.WebAssembly.csproj"  -c Release -o /app/publish

FROM build AS publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatGPT.WebAssembly.dll"]