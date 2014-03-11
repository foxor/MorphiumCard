using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class LoadUp : Effect
    {
        protected static DynamicProvider EngineMag = () => 1;
        protected static DynamicProvider WeightMag = () => 3;

        public LoadUp(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return EngineMag;
            yield return WeightMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Morphid;
        }

        public override void Apply(string guid)
        {
            GameState.AddEngine(guid, EngineMag());
            GameState.AddWeight(guid, WeightMag());
        }

        public override int Cost()
        {
            return 0;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.All;
        }
    }
}
