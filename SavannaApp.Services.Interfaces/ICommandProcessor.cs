using SavannaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Interfaces
{
    public interface ICommandProcessor
    {
        Command Command{ get;  }

        void Process();

        public void ClearInputBuffer();

        public void LogKey();
    }
}
