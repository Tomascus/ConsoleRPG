using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Decorators
{
    // This class is the base class for all item decorators - it implements the IItemDecorator interface 
    // It has a wrappedItem field that is used to wrap any IItem 
    internal abstract class ItemDecorator : IItemDecorator, IWeapon
    {
        protected IItem wrappedItem; 

        public ItemDecorator(IItem item)
        {
            wrappedItem = item;
        }

        // The properties of the wrapped item are overridden to include the effect of the decorator - powerful, unlucky etc
        public virtual string Name => $"{wrappedItem.Name} {GetEffectName()}";
        public virtual char Symbol => wrappedItem.Symbol;
        public virtual string Description => $"{wrappedItem.Description} {GetEffectDescription()}";


        // These methods are abstract and must be implemented by the concrete decorators - powerful decorator, unlucky decorator etc
        protected abstract string GetEffectName();
        protected abstract string GetEffectDescription();

        public abstract void ApplyEffectsToPlayer(Player player);
        public abstract void RemoveEffectsFromPlayer(Player player);

        // Uses the visitor pattern to apply the effects of the item to the player if the item is a weapon
        public virtual int AcceptAttack(IAttackVisitor visitor, Player player)
        {
            if (wrappedItem is IWeapon weapon)
            {
                return weapon.AcceptAttack(visitor, player);
            }
            else
            {
                // If wrapped item is not a weapon but has damage we will use non weapon visitor
                return visitor.VisitNonWeapon(this, player);
            }
        }

        // Works the same as the AcceptAttack method but for defense
        public virtual int AcceptDefense(IDefenseVisitor visitor, Player player)
        {
            if (wrappedItem is IWeapon weapon)
            {
                return weapon.AcceptDefense(visitor, player);
            }
            else
            {
                // If wrapped item is not a weapon
                return visitor.VisitNonWeapon(this, player);
            }
        }
    }
}
