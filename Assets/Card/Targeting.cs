using UnityEngine;
using System.Collections;

public class TargetingRequirements {
	public enum TargetTypeFlags {
		Lane = 1 << 0,
		Morphid = 1 << 1,
		Minion = 1 << 2,
		Enemy = 1 << 3,
		Friendly = 1 << 4
	}
}

public class Target {
}
