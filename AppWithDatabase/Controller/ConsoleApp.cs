using AppWithDatabase.Models;
using Spectre.Console;

namespace AppWithDatabase.Controller;

public class ConsoleApp
{
    private readonly MeasurementsContext _db;

    public ConsoleApp(MeasurementsContext dbContext)
    {
        _db = dbContext;
    }

    public async Task Run()
    {
        while (true)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a [green]CRUD[/] operation to execute on table [blue]Measurements[/]")
                    .PageSize(5)
                    .AddChoices("Create", "Read", "Update", "Delete", "Exit"));


            switch (option)
            {
                case "Create":
                    await Create();
                    break;
                case "Read":
                    Read();
                    break;
                case "Update":
                    await Update();
                    break;
                case "Delete":
                    await Delete();
                    break;
                case "Exit":
                    return;
            }
        }
    }

    public async Task Create()
    {
        //User prompts with values
        var userId = AnsiConsole.Ask<int>("Enter [green]UserId[/]: ");
        var sensorId = AnsiConsole.Ask<int>("Enter [green]SensorId[/]: ");
        var temperature = AnsiConsole.Ask<float>("Enter [green]Temperature[/]: ");
        AnsiConsole.Clear();

        // Create a Table with the Inputted Data
        AnsiConsole.Write(
            new Table()
            .AddColumn(new TableColumn("[yellow]UserId[/]").Centered())
            .AddColumn(new TableColumn("[yellow]SensorId[/]").Centered())
            .AddColumn(new TableColumn("[yellow]Temperature[/]").Centered())
            .AddRow($"[green]{userId}[/]", $"[green]{sensorId}[/]", $"[green]{temperature}[/]")
            );

        //Prompt the user if they really want to insert the new data
        var proceedWithData = AnsiConsole.Prompt(
            new SelectionPrompt<bool> { Converter = value => value ? "[green]Yes[/]" : "[red]No[/]" }
                .Title("Do you want to insert this data into table [blue]Measurements[/]?")
                .AddChoices(true, false));
        if (!proceedWithData) return;

        //Create a new Measurement with the given data
        var measurement = new Measurement()
        {
            UserId = userId,
            SensorId = sensorId,
            Temperature = temperature
        };

        //Insert the data
        var result = await _db.AddAsync(measurement);
        await _db.SaveChangesAsync();
        Console.WriteLine("Measurement created successfully.");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }

    public void Read()
    {
        var measurements = _db.Measurements.ToList();


        //Show all the measurements
        ConsoleHandler.ShowMeasurements(measurements);

        //Prompt to stop reading
        AnsiConsole.Prompt(
            new SelectionPrompt<bool> { Converter = _ => "[green]Continue[/]" }
                .Title("Press [blue]<enter>[/] to Continue..")
                .AddChoices(true));
        AnsiConsole.Clear();
    }


    public async Task Update()
    {
        var measurements = _db.Measurements.ToList();

        ConsoleHandler.ShowMeasurements(measurements);

        //prompt the user to select the table they want to update. Returns the index of the selected table.
        var selectedTableId = ConsoleHandler.SelectTablePrompt(measurements);
        var selectedMeasurement = measurements.FirstOrDefault(item => item.Id == selectedTableId);

        //Create the updated Measurement with the users Input
        if (selectedMeasurement == null) return;

        var updatedMeasurement = ConsoleHandler.MultiSelectUpdatePrompt(selectedMeasurement);

        //Table that shows original and updated table
        ConsoleHandler.ShowChangesTable(selectedMeasurement, updatedMeasurement);

        // Are you sure prompt
        var changeMeasurement = AnsiConsole.Prompt(
            new SelectionPrompt<bool> { Converter = value => value ? "[green]Yes[/]" : "[red]No[/]" }
                .Title("Are you sure you want to [lime]UPDATE[/] [blue]Measurements[/]?")
                .AddChoices(true, false));
        if (!changeMeasurement) return;

        //Update changes to database
        selectedMeasurement.UserId = updatedMeasurement.UserId;
        selectedMeasurement.SensorId = updatedMeasurement.SensorId;
        selectedMeasurement.Temperature = updatedMeasurement.Temperature;

        _db.Measurements.Update(selectedMeasurement);
        await _db.SaveChangesAsync();
        Console.WriteLine("Measurement updated successfully");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }


    public async Task Delete()
    {
        //show all measurements
        var measurements = _db.Measurements.ToList();
        ConsoleHandler.ShowMeasurements(measurements);

        //multi select prompt to get a list of tables to delete
        var selectedMeasurementsIdList = ConsoleHandler.MultiSelectDeletePrompt(measurements);
        if (selectedMeasurementsIdList.Count == 0) return;

        //Are you sure prompt
        var deleteMeasurement = AnsiConsole.Prompt(
            new SelectionPrompt<bool> { Converter = value => value ? "[green]Yes[/]" : "[red]No[/]" }
                .Title("Are you sure you want to [red]DELETE[/] the selected [blue]Measurements[/]?")
                .AddChoices(true, false));
        if (!deleteMeasurement) return;

        //loop through all the selected measurements and delete them from the database
        foreach (var measurement in selectedMeasurementsIdList.Select(measurementId => measurements[measurementId - 1]))
        {
            _db.Remove(measurement);
        }
        await _db.SaveChangesAsync();
        Console.WriteLine($"({selectedMeasurementsIdList.Count}) Measurements deleted successfully.");
        Thread.Sleep(2000);
        AnsiConsole.Clear();
    }
}
