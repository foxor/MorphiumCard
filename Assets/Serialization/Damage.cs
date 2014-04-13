using UnityEngine;
using System.Collections.Generic;

public class Damage {
    public string Target {get;set;}
    public string Source {get;set;}
    public int Magnitude {get;set;}
}

public class LaneDamage : Damage {
    public string EnemyMorphid {get;set;}
}