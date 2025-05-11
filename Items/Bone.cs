using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items
{
    internal class Bone : Item
    {
        public override string Name => "Bone";
        public override char Symbol => 'b';
        public override string Description => "Very old bone.";
    }
}
