FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY OOPLearn.Backend/TaskService/ OOPLearn.Backend/TaskService/

RUN dotnet restore "OOPLearn.Backend/TaskService/TaskServiceApp/TaskServiceApp.csproj"

RUN dotnet publish "OOPLearn.Backend/TaskService/TaskServiceApp/TaskServiceApp.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_Kestrel__Endpoints__Grpc__Url=http://+:5059
ENV ASPNETCORE_Kestrel__Endpoints__Grpc__Protocols=Http2

EXPOSE 5059

ENTRYPOINT ["dotnet", "TaskServiceApp.dll"]