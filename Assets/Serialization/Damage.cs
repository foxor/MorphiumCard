using UnityEngine;
using System.Collections.Generic;

public enum DamageType {
    Card,
    Ability,
    Attack
}

public class Damage {
    public string Target;
    public string Source;
    public int Magnitude;
    public DamageType Type;
}

public class LaneDamage : Damage {
    public string EnemyMorphid {get;set;}
}