using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class DrillBuddy : Effect
    {
        public static DynamicProvider AttackMag = () => GameState.ActiveMorphid.Weight;
        public static DynamicProvider DefenseMag = () => GameState.ActiveMorphid.Weight * 3;

        public DrillBuddy(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return AttackMag;
            yield return DefenseMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Empty;
            yield return TargetTypeFlag.Lane;
        }

        public override void Apply(string guid)
        {
            GameState.SummonMinion(guid, AttackMag(), DefenseMag(), false, false);
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
