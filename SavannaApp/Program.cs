using SavannaApp.Interfaces;
using SavannaApp.Services;
using SavannaApp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SavannaApp.Services.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
        services.AddHostedService<Worker>()
            .AddScoped<IGame, Game>()
            .AddScoped<IGameConsole, GameConsole>()
            .AddScoped<IAnimalField, AnimalField>()
            .AddScoped<ICommandProcessor, ConsoleCommandProcessor>()
            .AddScoped<IVisualizer, VisualizerToConsole>(x => 
                new (x.GetRequiredService<IGameConsole>(), x.GetRequiredService<IAnimalLoader>())
                {
                    CharacterForLion = "L",
                    CharacterForAntelope = "A",
                    CharacterForEmpty = "◌"
                })
            .AddScoped<IBreeder, AnimalBreeder>()
            .AddScoped<IAnimalLoader, AnimalLoaderFromAssembly>( x => new ("SavannaApp.Animals"))
    ).Build();

host.Run();
