using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public enum Slot {
    [SlotOrder(0)]
    Head,
    
    [SlotOrder(1)]
    Chest,
    
    [SlotOrder(2)]
    Arm,
    
    [SlotOrder(3)]
    Leg
}

public static class SlotExtension {
    public static int Order (this Slot s) {
        FieldInfo fi = typeof(Slot).GetField(s.ToString());
        foreach (SlotOrder order in fi.GetCustomAttributes(typeof(SlotOrder), false) as SlotOrder[]) {
            return order.order;
        }
        throw new Exception ("Slot does not specify list location");
    }
}

[AttributeUsage (AttributeTargets.Field)]
public class SlotOrder : Attribute {
    public int order;

    public SlotOrder (int order) {
        this.order = order;
    }
}