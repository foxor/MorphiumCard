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
public abstract class Lie : SignalData {
    [SerializeField]
    [ProtoMember(1)]
    public Facet
        Facet;
    
    public virtual bool Applies (params object[] args) {
        return true;
    }
    
    public abstract object TellTale ();
    
    public override void OnQueue () {
        Facade.AddLie(this);
    }
    
    public override void OnDequeue () {
        Facade.RemoveLie(this);
    }
}