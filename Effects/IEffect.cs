using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Models.Effects
{
    internal interface IEffect
    {
        string Name { get; }
        string Description { get; }
        bool IsExpired { get; }
        void Apply(Player player);
        void Remove(Player player);
        void Update(); // Called each turn
        string GetStatusText(); 
    }
}
