using SavannaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Interfaces
{
    public interface IVisualizer
    {
        void Visualize(AnimalFieldModel animalFieldModel);
    }
}
