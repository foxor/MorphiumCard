using UnityEngine;
using System.Collections;

public class AggressiveSalvage : Effect {
    public static DamageProvider damageMag = new ActiveMorphidDamageProvider(4);
	public static DynamicProvider parts = () => 6;

	public AggressiveSalvage (string text) : base (text){}

	protected override System.Collections.Generic.IEnumerable<DynamicProvider> TemplatingArguments ()
	{
        yield return damageMag.Provider;
		yield return parts;
	}

	protected override System.Collections.Generic.IEnumerable<TargetTypeFlag> TargetTypeFlags ()
	{
		yield return TargetTypeFlag.Enemy;
		yield return TargetTypeFlag.Friendly;
		yield return TargetTypeFlag.Morphid;
		yield return TargetTypeFlag.Minion;
	}

	public override void Apply (string guid)
	{
        damageMag.Apply(guid);
		GameState.AddParts(GameState.ActiveMorphid.GUID, parts());
	}

	public override int Cost ()
	{
		return 8;
	}

	public override TargetingType TargetingType ()
	{
		return global::TargetingType.Single;
	}
}