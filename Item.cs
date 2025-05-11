using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    // Abstract class that provides basic implementation for items 
    internal abstract class Item : IItem
    {
        public virtual string Name { get; }
        public virtual char Symbol { get; }
        public virtual string Description { get; }
    }
}
