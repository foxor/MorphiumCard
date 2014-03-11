using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class ExoTruck : Effect
    {
        protected static DynamicProvider AttackMag = () => 2;
        protected static DynamicProvider DefenseMag = () => 7;

        public ExoTruck(string text) : base(text) { }

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
            GameState.SummonMinion(guid, AttackMag(), DefenseMag(), false);
        }

        public override int Cost()
        {
            return 3;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.Single;
        }
    }
}
