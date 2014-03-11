using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class RuggedSuspension : Effect
    {
        protected static DynamicProvider RepairMag = () => 4;
        protected static DynamicProvider WeightMag = () => 4;

        public RuggedSuspension(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return RepairMag;
            yield return WeightMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Morphid;
        }

        public override void Apply(string guid)
        {
            GameState.RepairGuid(guid, RepairMag());
            GameState.AddWeight(guid, WeightMag());
        }

        public override int Cost()
        {
            return 6;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.All;
        }
    }
}
