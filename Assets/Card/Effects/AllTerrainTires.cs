using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class AllTerrainTires : Effect
    {
        public AllTerrainTires(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield break;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Minion;
        }

        public override void Apply(string guid)
        {
            GameState.GetMinion(guid).Protect = true;
        }

        public override int Cost()
        {
            return 4;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.All;
        }
    }
}
