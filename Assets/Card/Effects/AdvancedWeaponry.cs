using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class AdvancedWeaponry : Effect
    {
        protected static DynamicProvider DamageMag = () => GameState.ActiveMorphid.Morphium + GameState.ActiveMorphid.DamageBonus;

        public AdvancedWeaponry(string text) : base(text) { }

        protected override IEnumerable<DynamicProvider> TemplatingArguments()
        {
            yield break;
        }

        protected override IEnumerable<TargetTypeFlag> TargetTypeFlags()
        {
            yield return TargetTypeFlag.Friendly;
            yield return TargetTypeFlag.Morphid;
        }

        public override void Apply(string guid)
        {
            GameState.SetResearch(guid, () =>
            {
                TargetingRequirements req = new TargetingRequirements(
                    (int)TargetTypeFlag.Enemy | (int)TargetTypeFlag.Morphid,
                    global::TargetingType.All
                );
                foreach (string morphid in req.ChosenTargets(null))
                {
                    GameState.DamageGuid(morphid, guid, DamageMag());
                }
                GameState.ActiveMorphid.Morphium = 0;
            });
        }

        public override int Cost()
        {
            return 10;
        }

        public override TargetingType TargetingType()
        {
            return global::TargetingType.All;
        }
    }
}
