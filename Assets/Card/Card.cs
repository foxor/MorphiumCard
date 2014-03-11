using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public sealed class Card {
    [SerializeField]
    public Slot Slot;

    [NonSerialized]
    //Prevents unity from copying the guids around
    [ProtoMember(1)]
    public string GUID;

    [SerializeField]
    [ProtoMember(2)]
    public String Name;

    [SerializeField]
    [ProtoMember(3)]
    public String Text;

    [SerializeField]
    [ProtoMember(4)]
    public String Manufacturer;

    [SerializeField]
    [ProtoMember(5)]
    public int Cost;

    [SerializeField]
    [ProtoMember(6)]
    public bool Charged;

	[SerializeField]
	[ProtoMember(7)]
	public string[] TargetableGuids;

	[SerializeField]
	[ProtoMember(8)]
	public TargetingType TargetingType;

    public Effect Effect;
    
    public Card () {
        GUID = Guid.NewGuid().ToString();
        Effect = null;
    }
    
    public void Process (string pickedGuid) {
        Morphid self = GameState.ActiveMorphid;
        if (self.Morphium >= Cost) {
            self.Morphium -= Cost;
            TargetingRequirements req = new TargetingRequirements(Effect);
            IEnumerable<string> targets = req.ChosenTargets(pickedGuid);
            if (!targets.Any()) {
                throw new TargetingException("Client picked no valid targets for effect: " + Effect.ToString());
            }
			foreach (string guid in targets) {
            	Effect.Apply(guid);
			}
        }
    }

    public Card Copy() {
        Card r = this.SerializeProtoBytes().DeserializeProtoBytes<Card>();
        r.Effect = Effect;
        return r;
    }

    public void Template() {
        this.Cost = Effect.Cost();
        this.Text = Effect.Text;
		TargetingRequirements req = new TargetingRequirements(Effect);
		this.TargetableGuids = req.AllowedTargets().ToArray();
		this.TargetingType = Effect.TargetingType();
    }
}