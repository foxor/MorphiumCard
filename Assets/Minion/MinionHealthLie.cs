using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class MinionHealthLie : Lie {
    [SerializeField]
    [ProtoMember(2)]
    public int
        Health;
    [SerializeField]
    [ProtoMember(3)]
    public string
        GUID;

    public MinionHealthLie () {
        Facet = Facet.MinionHealth;
    }

    public override bool Applies (params object[] args) {
        return ((string)args[0]) == GUID;
    }

    public override object TellTale () {
        return Health;
    }
}