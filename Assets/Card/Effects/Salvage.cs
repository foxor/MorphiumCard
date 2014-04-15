using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class Salvage : Effect
    {
        public Salvage(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield break;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Minion;
        }

        public override void Apply(string guid)
        {
            Minion minion = GameState.GetMinion(guid);
            if (minion != null)
            {
                GameState.ActiveMorphid.Weight += minion.Durability;
                GameState.DestroyMinion(guid);
            }
        }

        public override int Cost()
        {
            return 2;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.Single;
        }
    }
}
