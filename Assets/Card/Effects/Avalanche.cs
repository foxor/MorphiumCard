using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class Avalanche : Effect
    {
        public static DynamicProvider DamageMag = () => 7 + GameState.ActiveMorphid.DamageBonus;

        public Avalanche(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return DamageMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Enemy;
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Minion;
        }

        public override void Apply(string guid)
        {
            GameState.DamageGuid(guid, GameState.ActiveMorphid.GUID, DamageMag());
        }

        public override int Cost()
        {
            return 4;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.Single;
        }
    }
}
