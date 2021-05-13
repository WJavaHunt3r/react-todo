# React - ASP .NET Todo App
A todo App where the frontend is made in react, while the backend is powered by ASP .Net 5.0, and the data is stored in MSSql.

## What you will need

A labor elvégzéséhez szükséges eszközök:

- Microsoft SQL Server (LocalDB or Express edition)
- SQL Server Management Studio
- Visual Studio 2019 v16.8 (or newer) with .NET 5 SDK installed+
- Visual Studio Code

## Getting started
Download and unzip the folder to your desired fodler. The frontend is in the my-todo folder. Read the `Readme` there to install the React app.

## Create database
In order to store the todos permanently, we will need a database. Now go ahaed ang create a knew one in you desired server manager. If you don't have one:
1. Install Microsoft SQL Server and SQL Server Management Studi
2. Open SQL Server Management and connect to the database:
      
      - Server name: `(localdb)\mssqllocaldb` or `.\sqlexpress`
      - Authentication: `Windows authentication`
3. Create a new DataBase with the name: `Todos`

## The backend
The .NET App is located in the raect-todo (misspell) folder. Open it and start the `raect-todo.sln` file. 
In the `Soulution Explorer` click the ReactTodo.Api and in the dropdown find the file `appsettings.json`.
Here you can find the connecting string. If you do not use MSSQL or you changed the name of the database, you can configure it here.

## Start the backend
After some seconds a new webside should open on : [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)
Here you can alredy test the backend

## You are ready to go
