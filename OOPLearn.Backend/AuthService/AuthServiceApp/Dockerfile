FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY OOPLearn.Backend/AuthService/ OOPLearn.Backend/AuthService/

RUN dotnet restore "OOPLearn.Backend/AuthService/AuthServiceApp/AuthServiceApp.csproj"

RUN dotnet publish "OOPLearn.Backend/AuthService/AuthServiceApp/AuthServiceApp.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_Kestrel__Endpoints__Grpc__Url=http://+:5060
ENV ASPNETCORE_Kestrel__Endpoints__Grpc__Protocols=Http2

EXPOSE 5060

ENTRYPOINT ["dotnet", "AuthServiceApp.dll"]