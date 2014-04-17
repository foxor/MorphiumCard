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
        public static DynamicProvider WeightGain = () => 2;

        public DrillBuddy(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return AttackMag;
            yield return DefenseMag;
            yield return WeightGain;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Lane;
        }

        public override void Apply(string guid)
        {
            GameState.SummonMinion(guid, AttackMag(), DefenseMag(), Name, new MinionBuilder());
            GameState.AddWeight(GameState.ActiveMorphid.GUID, WeightGain());
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
