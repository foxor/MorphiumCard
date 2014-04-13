using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class RollOver : Effect
    {
        protected static DamageProvider DamageMag = new ActiveMorphidDamageProvider(3, () => GameState.ActiveMorphid.Weight);

        public RollOver(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return DamageMag.Provider;
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
            DamageMag.Apply(guid);
        }

        public override int Cost()
        {
            return 5;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.Single;
        }
    }
}
