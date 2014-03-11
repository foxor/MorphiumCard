using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class RegenerativeSuspension : Effect
    {
        protected static DynamicProvider RepairMag = () => 4;
        protected static DynamicProvider EngineMag = () => -1;

        public RegenerativeSuspension(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return RepairMag;
            yield return EngineMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Morphid;
        }

        public override void Apply(string guid)
        {
            GameState.AddEngineSequence(guid, () => {
                GameState.RepairGuid(guid, RepairMag());
            });
            GameState.AddEngine(guid, EngineMag());
        }

        public override int Cost()
        {
            return 7;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.All;
        }
    }
}
