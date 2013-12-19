using UnityEngine;
using System.Collections;

public enum TargetTypeFlag {
    Lane = 1 << 0,
    Morphid = 1 << 1,
    Minion = 1 << 2,
    Enemy = 1 << 3,
    Friendly = 1 << 4,
    Empty = 1 << 5,
    Random = 1 << 6
}