using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class Quake : Effect
    {
        public static DamageProvider DamageMag = new ActiveMorphidDamageProvider(7);

        public Quake(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return DamageMag.Provider;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Enemy;
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Minion;
        }

        public override void Apply(string guid)
        {
            DamageMag.Apply(guid);
        }

        public override int Cost()
        {
            return 9;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.All;
        }
    }
}
