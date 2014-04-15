using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class SpareParts : Effect
    {
        protected static DynamicProvider AttackMag = () => GameState.ActiveMorphid.Weight / 2;
        protected static DynamicProvider DefenseMag = () => GameState.ActiveMorphid.Weight * 2;
		protected static DynamicProvider WeightMag = () => 2;

        public SpareParts(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return AttackMag;
            yield return DefenseMag;
			yield return WeightMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Empty;
            yield return TargetTypeFlag.Lane;
        }

        public override void Apply(string guid)
        {
            GameState.SummonMinion(guid, AttackMag(), DefenseMag(), Name, new MinionBuilder());
			GameState.AddWeight(GameState.ActiveMorphid.GUID, WeightMag());
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
