using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Models.Effects
{
    internal abstract class TemporaryEffect : IEffect
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsExpired => RemainingTurns <= 0;

        protected int StartDuration;
        protected int RemainingTurns;

        public TemporaryEffect(string name, string description, int duration)
        {
            Name = name;
            Description = description;
            StartDuration = duration;
            RemainingTurns = duration;
        }

        public abstract void Apply(Player player);
        public abstract void Remove(Player player);
        public virtual void Update()
        {
            RemainingTurns--;
        }

        public virtual string GetStatusText()
        {
            return $"{Name}: {Description} ({RemainingTurns} turns remaining)";
        }
    }
}
