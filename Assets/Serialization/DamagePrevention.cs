using UnityEngine;
using System;
using System.Collections.Generic;

public static class DamagePrevention {
    public static Action<Damage> PreventDamage = (Damage damage) => {};
}