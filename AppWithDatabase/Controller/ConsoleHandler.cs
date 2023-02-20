
using AppWithDatabase.Models;
using Spectre.Console;

namespace AppWithDatabase.Controller;

public class ConsoleHandler
{
    public static void ShowMeasurements(List<Measurement> measurements)
    {
        var promptTable = new Table()
            .Alignment(Justify.Left)
            .Title("Tables")
            .AddColumn(new TableColumn("Id").Centered())
            .AddColumn(new TableColumn("UserId").Centered())
            .AddColumn(new TableColumn("SensorId").Centered())
            .AddColumn(new TableColumn("Temperature").Centered());

        foreach (var measurement in measurements)
        {

            promptTable.AddRow($"{measurement.Id}", $"{measurement.UserId}", $"{measurement.SensorId}", $"{measurement.Temperature}");
        }

        // Show the Table
        AnsiConsole.Write(promptTable);
    }

    public static long SelectTablePrompt(List<Measurement> measurements)
    {

        // Build the prompt
        var prompt = new SelectionPrompt<long>
        {
            Converter = id => $"Table ID: [green]{id}[/]"
        };
        prompt.Title("Select the table to [green]Update[/]");
        foreach (var id in measurements.Select(measurement => measurement.Id))
        {
            prompt.AddChoice(id);
        }

        // Show the prompt
        var selectedPromptId = AnsiConsole.Prompt(prompt);
        return selectedPromptId;
    }

    public static List<int> MultiSelectDeletePrompt(List<Measurement> measurements)
    {
        var measurementNumberList = new List<int>();
        var allChoices = new List<string>();

        var prompt = new MultiSelectionPrompt<string>()
            .Title("Select [blue]Measurements[/] by Table Number")
            .NotRequired() // Not required to have a favorite fruit
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to see more tables)[/]")
            .InstructionsText(
                "[grey](Press [blue]<space>[/] to [green]Select[/], " +
                "[blue]<enter>[/] to accept)[/]");
        for (var index = 0; index < measurements.Count; index++)
        {
            var tableNr = index + 1;
            allChoices.Add($"{tableNr}");
        }
        prompt.AddChoiceGroup("select all", allChoices);


        var selectedMeasurements = AnsiConsole.Prompt(prompt);
        // Write the selected fruits to the terminal
        foreach (var measurement in selectedMeasurements)
        {
            var result = int.Parse(measurement);
            measurementNumberList.Add(result);
        }
        AnsiConsole.Clear();
        return measurementNumberList;
    }

    public static Measurement MultiSelectUpdatePrompt(Measurement measurement)
    {
        var userId = measurement.UserId;
        var sensorId = measurement.SensorId;
        var temperature = measurement.Temperature;

        var selectedValues = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
            .Title("Select [blue]Values[/] to update")
            .NotRequired() // Not required to have a favorite fruit
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to see more tables)[/]")
            .InstructionsText(
                "[grey](Press [blue]<space>[/] to [green]Select[/], " +
                "[blue]<enter>[/] to accept)[/]")
            .AddChoiceGroup("select all", "UserId", "SensorId", "Temperature"));
        // Write the selected fruits to the terminal

        foreach (var value in selectedValues)
        {
            switch (value)
            {
                case "UserId":
                    userId = AnsiConsole.Ask<int>("[green]UserId[/]: ");
                    break;
                case "SensorId":
                    sensorId = AnsiConsole.Ask<int>("[green]SensorId[/]: ");
                    break;
                case "Temperature":
                    temperature = AnsiConsole.Ask<float>("[green]Temperature[/]: ");
                    break;
            }
        }
        var updatedMeasurement = new Measurement
        {
            Id = measurement.Id,
            UserId = userId,
            SensorId = sensorId,
            Temperature = temperature
        };
        AnsiConsole.Clear();
        return updatedMeasurement;
    }

    public static void ShowChangesTable(Measurement selectedMeasurement, Measurement updatedMeasurement)
    {
        //Table to show the differences
        var orgTable = new Table()
            .AddColumn("UserId").Centered()
            .AddColumn("SensorId").Centered()
            .AddColumn("Temperature").Centered()
            .AddRow(
                $"[gray]{selectedMeasurement.UserId}[/]",
                $"[gray]{selectedMeasurement.SensorId}[/]",
                $"[gray]{selectedMeasurement.Temperature}[/]");

        var updatedTable = new Table()
            .AddColumn("UserId").Centered()
            .AddColumn("SensorId").Centered()
            .AddColumn("Temperature").Centered()
            .AddRow(
                $"[lime]{updatedMeasurement.UserId}[/]",
                $"[lime]{updatedMeasurement.SensorId}[/]",
                $"[lime]{updatedMeasurement.Temperature}[/]");

        var table = new Table().Title("Table Changes")
            .AddColumn(new TableColumn("[yellow]Original[/]").Centered())
            .AddColumn(new TableColumn("[lime]Updated[/]").Centered())
            .AddRow(orgTable, updatedTable);

        AnsiConsole.Write(table);
    }
}