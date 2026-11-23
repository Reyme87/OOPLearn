FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY OOPLearn.Backend/LessonService/ OOPLearn.Backend/LessonService/

RUN dotnet restore "OOPLearn.Backend/LessonService/LessonServiceApp/LessonServiceApp.csproj"

RUN dotnet publish "OOPLearn.Backend/LessonService/LessonServiceApp/LessonServiceApp.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_Kestrel__Endpoints__Grpc__Url=http://+:5290
ENV ASPNETCORE_Kestrel__Endpoints__Grpc__Protocols=Http2

EXPOSE 5290

ENTRYPOINT ["dotnet", "LessonServiceApp.dll"]