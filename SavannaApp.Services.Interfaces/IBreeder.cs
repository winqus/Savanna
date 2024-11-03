using SavannaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Services.Interfaces
{
    public interface IBreeder
    {
        int BreedingDistance { get; set; }

        void Initialize(int breedingDistance);

        void Update();
    }
}
