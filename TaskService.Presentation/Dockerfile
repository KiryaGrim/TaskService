FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TaskService/TaskService.Presentation/TaskService.Presentation.csproj", "TaskService/TaskService.Presentation/"]

RUN dotnet restore "TaskService/TaskService.Presentation/TaskService.Presentation.csproj"

COPY . .

WORKDIR "/src/TaskService/TaskService.Presentation"
RUN dotnet publish "TaskService.Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskService.Presentation.dll"]