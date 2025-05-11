using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items
{
    internal class Book : Item
    {
        public override string Name => "Book";
        public override char Symbol => 'b';
        public override string Description => "An ancient book.";
    }
}
