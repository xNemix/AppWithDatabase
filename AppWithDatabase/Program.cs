using AppWithDatabase.Controller;
using AppWithDatabase.Controller.Repositories;
using Spectre.Console;
using System.Data.SqlClient;

try
{
    // connection string
    const string connectionString =
        @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Measurements;Integrated Security=True;";

    // Create SQL Repo
    var repo = new SqlMeasurementRepository(connectionString);

    // Create A New Console App And Pass the Repo
    var consoleApp = new ConsoleApp(repo);

    //Start All Services And Run The Console Application

    // StartServices().Wait(); // This sucks when developing the app....

    consoleApp.Run();
}
catch (SqlException e)
{
    Console.WriteLine(e.ToString());
}

Console.WriteLine("Shutting down the application...");
Thread.Sleep(2000);

async Task StartServices()
{
    const string connectionString =
        @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Measurements;Integrated Security=True;";

    var rand = new Random();

    // SQL SERVICES
    await AnsiConsole.Status()
        .StartAsync("Starting SQL Services...", ctx =>
        {
            ctx.Spinner(Spinner.Known.Dots);
            ctx.SpinnerStyle(Style.Parse("lime"));
            Thread.Sleep(rand.Next(2000, 4000));


            ctx.Status("Gathering Information...");
            Thread.Sleep(rand.Next(2000, 4000));
            AnsiConsole.MarkupLine("[[SQL Service]] Connection String: [green]Valid[/]");
            Thread.Sleep(rand.Next(1000, 2000));
            AnsiConsole.MarkupLine("[[SQL Service]] Database Name: [green]Measurements[/]");


            //Connect to Database
            ctx.Status("Connecting to database...");
            Thread.Sleep(rand.Next(2000, 4000));
            AnsiConsole.MarkupLine("[[SQL Service]] Database Connection: [green]Connected[/]");
            return Task.CompletedTask;
        });


    // Application
    await AnsiConsole.Status()
        .StartAsync("Starting Application...", ctx =>
        {
            ctx.Status("Loading UI...");
            ctx.Spinner(Spinner.Known.Dots);
            ctx.SpinnerStyle(Style.Parse("lime"));
            Thread.Sleep(rand.Next(2000, 4000));
            AnsiConsole.MarkupLine("[[Application]] UI Elements: [green]Loaded[/]");
            Thread.Sleep(rand.Next(1000, 2000));
            AnsiConsole.MarkupLine("[[Application]] Status: [green]Started[/]");
            return Task.CompletedTask;
        });

    Thread.Sleep(3000);
    AnsiConsole.Clear();
}

