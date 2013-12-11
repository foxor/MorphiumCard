using UnityEngine;
using System.Collections;

public class Defensive : Effect {
    public const string CSV_NAME = "Defensive";

    public override void Apply (string TargetGuid) {
    }

    public override int Targeting () {
        return (int)TargetTypeFlag.Empty | (int)TargetTypeFlag.Lane;
    }

    public override TargetingType TargetingType () {
        return global::TargetingType.All;
    }
}