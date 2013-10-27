using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public enum Slot {
	[SlotOrder(1)]
	Head,
	
	[SlotOrder(2)]
	Chest,
	
	[SlotOrder(3)]
	Arm,
	
	[SlotOrder(4)]
	Legs
}

public static class SlotExtension {
	public static int Order(this Slot s) {
		FieldInfo fi = typeof(Slot).GetField(s.ToString());
		foreach (SlotOrder order in fi.GetCustomAttributes(typeof(SlotOrder), false) as SlotOrder[]) {
			return order.order;
		}
		throw new Exception("Slot does not specify list location");
	}
}

[AttributeUsage (AttributeTargets.Field)]
public class SlotOrder : Attribute {
    public int order;
    public SlotOrder(int order) {
        this.order = order;
    }
}