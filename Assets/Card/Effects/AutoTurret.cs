using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class AutoTurret : Effect
    {
        protected static DynamicProvider AttackMag = () => 10;
        protected static DynamicProvider DefenseMag = () => 1;

        public AutoTurret(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return AttackMag;
            yield return DefenseMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Lane;
        }

        public override void Apply(string guid)
        {
            GameState.SummonMinion(guid, AttackMag(), DefenseMag(), Name, new MinionBuilder(){Defensive = true});
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
