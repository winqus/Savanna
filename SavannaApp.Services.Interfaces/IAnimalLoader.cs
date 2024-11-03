using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Services.Interfaces
{
    public interface IAnimalLoader
    {
        List<Type> GetAnimalTypes();

        void LoadAnimalTypes();

        bool HasType(Type type);
    }
}
