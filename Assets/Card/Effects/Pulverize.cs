using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class Pulverize : Effect
    {
        public static DynamicProvider DamageMag = () => 2 + GameState.ActiveMorphid.Weight * 2 + GameState.ActiveMorphid.DamageBonus;

        public Pulverize(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return DamageMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Enemy;
            yield return TargetTypeFlag.Minion;
            yield return TargetTypeFlag.Morphid;
        }

        public override void Apply(string guid)
        {
            GameState.DamageGuid(guid, GameState.ActiveMorphid.GUID, DamageMag());
        }

        public override int Cost()
        {
            return 7;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.Single;
        }
    }
}
