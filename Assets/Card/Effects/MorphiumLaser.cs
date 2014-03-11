using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class MorphiumLaser : Effect
    {
        protected static DynamicProvider DamageMag = () => (int)Math.Round(Math.Pow(Math.Pow(30.0, 1.0 / 9.0), (double)GameState.ActiveMorphid.Engine - 1.0));

        public MorphiumLaser(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return DamageMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Enemy;
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Minion;
            yield return TargetTypeFlag.Morphid;
        }

        public override void Apply(string guid)
        {
            GameState.DamageGuid(guid, DamageMag());
        }

        public override int Cost()
        {
            return 8;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.Single;
        }
    }
}
