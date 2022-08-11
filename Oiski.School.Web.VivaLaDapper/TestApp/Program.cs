using Microsoft.Extensions.Configuration;
using Oiski.School.Web.VivaLaDaper;
using TestApp;

var builder = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json");

var configuration = builder.Build();

var context = new SampleContext(configuration);

var sample = new SampleEntity { ID = 5, Title = "Inserted" };

string setupString = "Succeeded";

Console.WriteLine($"{setupString} (Add)     : {await context.AddAsync(sample)}");
sample.Title = "Updated";
Console.WriteLine($"{setupString} (Update)  : {await context.UpdateAsync(sample)}");
Console.WriteLine($"{setupString} (Delete)  : {await context.RemoveAsync(sample)}");