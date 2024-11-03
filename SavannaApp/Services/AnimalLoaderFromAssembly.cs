using SavannaApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Services
{
    public class AnimalLoaderFromAssembly : IAnimalLoader
    {
        public string AssemblyName { get; set; }

        protected List<Type> animalTypes;

        public AnimalLoaderFromAssembly(string assemblyName)
        {
            animalTypes = new List<Type>();
            AssemblyName = assemblyName;
        }

        public void LoadAnimalTypes()
        {
            animalTypes = Assembly.Load(AssemblyName).GetTypes()
                .Where(x => x.Namespace.Equals("SavannaApp.Animals"))
                .OrderBy(x => x.Name)
                .ToList();

            KeepTypesOnlyWithUniqueFirstLetterOfNames();
        }

        public List<Type> GetAnimalTypes()
        {
            return animalTypes;
        }

        private void KeepTypesOnlyWithUniqueFirstLetterOfNames()
        {
            var usableLetters = animalTypes.Select(x => x.Name[0]).Distinct().ToList();
            var newTypeList = new List<Type>();

            foreach(var type in animalTypes)
            {
                if (usableLetters.Contains(type.Name[0]))
                {
                    newTypeList.Add(type);
                    usableLetters.Remove(type.Name[0]);
                }
            }

            animalTypes = newTypeList;
        }

        public bool HasType(Type type)
        {
            return animalTypes.Contains(type);
        }
    }
}
