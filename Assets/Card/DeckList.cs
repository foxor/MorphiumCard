using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class DeckList {
    [SerializeField]
    [ProtoMember(1)]
    public string[] Cards = new string[0];
}
