using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class MorphiumBomb : Effect
    {
        protected static DynamicProvider EngineMag = () => -4;

        public MorphiumBomb(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield return EngineMag;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Morphid;
        }

        public override void Apply(string guid)
        {
            GameState.AddEngine(guid, EngineMag());

            TargetingRequirements req = new TargetingRequirements(
                (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Minion,
                global::TargetingType.All
            );
            foreach (string minionGuid in req.ChosenTargets(null))
            {
                GameState.DestroyMinion(minionGuid);
            }
        }

        public override int Cost()
        {
            return 8;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.All;
        }
    }
}
