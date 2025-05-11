using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    internal interface IDamageItems
    {
        int Damage { get; }
        bool IsTwoHanded { get; }
    }
}
