using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items
{
    internal class Sigil : Item
    {
        public override string Name => "Sigil";
        public override char Symbol => 's';
        public override string Description => "Strange symbol is shown.";
    }
}
