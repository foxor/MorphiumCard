using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class Attachment {
    
    [SerializeField]
    [ProtoMember(1)]
    public int RemainingHealth;
    
    [SerializeField]
    [ProtoMember(2)]
    public int RemainingCharges;
    
    [SerializeField]
    [ProtoMember(3)]
    public string Description;

    public Effect Effect;
    public Action OnDestroy;

    protected bool destroyed;
    public bool Destroyed {
        get {
            return destroyed;
        }
    }

    protected void CheckDestroy() {
        if (RemainingHealth <= 0 && RemainingCharges <= 0) {
            OnDestroy();
            destroyed = true;
        }
    }
    
    public void Damage(int damage) {
        RemainingHealth -= damage;
        CheckDestroy();
    }
    
    public void SpendCharge() {
        RemainingCharges -= 1;
        CheckDestroy();
    }

    public void Template() {
        TemplateStatus.Templating = true;

        Description = Effect.Text;
        
        TemplateStatus.Templating = false;
    }
}