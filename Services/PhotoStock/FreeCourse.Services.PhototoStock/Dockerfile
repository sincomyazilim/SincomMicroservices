FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/PhotoStock/FreeCourse.Services.PhototoStock/*.csproj Services/PhotoStock/FreeCourse.Services.PhototoStock/
RUN dotnet restore Services/PhotoStock/FreeCourse.Services.PhototoStock/*.csproj
COPY . .
RUN dotnet publish Services/PhotoStock/FreeCourse.Services.PhototoStock/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","FreeCourse.Services.PhototoStock.dll"]



