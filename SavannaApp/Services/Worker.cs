using SavannaApp.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SavannaApp.Services.Interfaces;

namespace SavannaApp
{
    public class Worker : BackgroundService
    {
        private readonly IGame _game;
        private readonly IAnimalLoader _animalLoader;

        public Worker(IGame game, IAnimalLoader animalLoader)
        {
            _game = game;
            _animalLoader = animalLoader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _animalLoader.LoadAnimalTypes();

            _game.StartGame();
            _game.RunGame();
            await Task.Yield();
        }
    }
}
