ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ENV ASPNETCORE_URLS=http://+:5001

EXPOSE 5001

WORKDIR /src

COPY ["../ToDoList.Frontend","ToDoList.Frontend/"]
COPY ["../ToDoList.Domain","ToDoList.Domain/"]

RUN dotnet restore "./ToDoList.Frontend/ToDoList.Frontend.csproj"
RUN dotnet restore "./ToDoList.Domain/ToDoList.Domain.csproj"
RUN dotnet build "./ToDoList.Frontend/ToDoList.Frontend.csproj"
RUN dotnet build "./ToDoList.Domain/ToDoList.Domain.csproj"
RUN dotnet publish "./ToDoList.Frontend/ToDoList.Frontend.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish -r linux-musl-x64
RUN dotnet publish "./ToDoList.Domain/ToDoList.Domain.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish -r linux-musl-x64

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet","ToDoList.Frontend.dll"]