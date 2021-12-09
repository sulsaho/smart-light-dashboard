# smart-light-dashboard

## Introduction

This repository contains the frontend and backend source code for a smart light dashboard application.

by\
 \
 Zillah Adahman\
 Angat Lamichanne\
 Sulayman Saho.

## Technology Overview

### Development

-   Frontend: [React](https://reactjs.org/docs/), [Material UI](https://material-ui.com/)
-   Backend: [C#](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/), [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)

### Packages (Frontend)

-   Axios for API calls
-   Victory for charting

### Packages (Backend)

-   Microsoft Entity FrameworkCore.Sqlite/5.0.11
-   NCorntab/3.3.1
-   Newtonsoft.Json/13.0.1
-   RestSharp/106.13.0
-   Swachbuckle.AspNetCore/5.6.3

### Prerequisites

-   [Node.js 10](https://nodejs.org/en/)
-   [.NET Core 5](https://dotnet.microsoft.com/download/dotnet-core/5.0)

### Start the backend .NET Core web API

```sh
cd backend
dotnet run
```

The backend API can be viewed at [http://localhost:5001/swagger](http://localhost:5001/swagger)

### Start the frontend React app

```sh
cd frontend
npm install
npm start
```

The frontend application will launch automatically at [http://localhost:3000](http://localhost:3000)

## Testing

The frontend test use [Jest](https://jestjs.io/). To run:

```sh
cd frontend
npm test
```

The backend test use [MSTest](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest). To run:

```sh
cd backend
dotnet test
```
