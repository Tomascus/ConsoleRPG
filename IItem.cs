using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    // This inferface defines the properties of items
    internal interface IItem
    {
        string Name { get; }
        char Symbol { get; }
        string Description { get; }
    }
}
