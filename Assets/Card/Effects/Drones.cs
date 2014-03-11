using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Card.Effects
{
    class Drones : Effect
    {
        protected static DynamicProvider AttackMag = () => GameState.ActiveMorphid.Morphium;
        protected static DynamicProvider DefenseMag = () => GameState.ActiveMorphid.Morphium * 3;

        public Drones(string text) : base(text) { }

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
            GameState.SetResearch(guid, () => {
                TargetingRequirements req = new TargetingRequirements(
                    (int)TargetTypeFlag.Random | (int)TargetTypeFlag.Empty | (int)TargetTypeFlag.Lane,
                    global::TargetingType.Single
                );
                foreach (string lane in req.ChosenTargets(null))
                {
                    GameState.SummonMinion(lane, AttackMag(), DefenseMag(), false);
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
