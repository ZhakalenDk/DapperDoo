using Microsoft.Extensions.Configuration;
using Oiski.School.Web.VivaLaDaper;
using TestApp;

var builder = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json");

var configuration = builder.Build();

var context = new SampleContext(configuration);

var sample = new SampleEntity { ID = 1, Title = "Inserted" };
var sample1 = new SampleEntity { ID = 2, Title = "Inserted 1" };
var extra = new ExtraEntity { Id = 1, Description = "Inserted" };
var extra1 = new ExtraEntity { Id = 2, Description = "Inserted 1" };

string setupString = "Succeeded";

//  Operation Sample
Console.WriteLine($"{setupString} (Add - Sample): {await context.AddAsync(sample)}");
Console.WriteLine($"{setupString} (Add - Sample1): {await context.AddAsync(sample1)}");

Console.ForegroundColor = ConsoleColor.Cyan;
foreach (var item in await context.GetAll<SampleEntity>())
{
    Console.WriteLine($"In Collection (Samples): {item.Title}");
}

sample.Title = "Updated Sample";
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine($"{setupString} (Update - Sample): {await context.UpdateAsync(sample)}");
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Found Sample: {(await context.GetByKeyAsync<SampleEntity, int>(1)).Title}");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($"{setupString} (Delete - Sample): {await context.RemoveAsync(sample)}");
Console.WriteLine($"{setupString} (Delete - Sample1): {await context.RemoveAsync(sample1)}");

Console.WriteLine();    //  Newline

//  Operation: Extra
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"{setupString} (Add - Extra): {await context.AddAsync(extra)}");
Console.WriteLine($"{setupString} (Add - Extra1): {await context.AddAsync(extra1)}");

Console.ForegroundColor = ConsoleColor.Cyan;
foreach (var item in await context.GetAll<ExtraEntity>())
{
    Console.WriteLine($"In Collection (Samples): {item.Description}");
}

extra.Description = "Updated Extra";
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine($"{setupString} (Update - Extra): {await context.UpdateAsync(extra)}");
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Found Extra: {(await context.GetByKeyAsync<ExtraEntity, int>(1)).Description}");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($"{setupString} (Delete - Extra): {await context.RemoveAsync(extra)}");
Console.WriteLine($"{setupString} (Delete - Extra1): {await context.RemoveAsync(extra1)}");
Console.ResetColor();