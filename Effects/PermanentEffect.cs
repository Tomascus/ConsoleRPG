using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Models.Effects
{
    internal abstract class PermanentEffect : IEffect 
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsExpired => false; 

        public PermanentEffect(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public abstract void Apply(Player player);
        public abstract void Remove(Player player);

        public virtual void Update()
        {
            
        }

        // This method is called to get status text of the effect
        public virtual string GetStatusText()
        {
            return $"{Name}: {Description} (Permanent)";
        }
    }
}
