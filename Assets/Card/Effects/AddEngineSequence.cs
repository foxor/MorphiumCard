using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

public class AddEngineSequence {
    public const string CSV_NAME = "AddEngineSequence";

    public Effect SequenceAddition;

    public  void Apply (string TargetGuid) {
        if (GameState.ActiveMorphid.EngineSequence == null) {
            //GameState.ActiveMorphid.EngineSequence = SequenceAddition.ToArray();
        }
        else {
            //GameState.ActiveMorphid.EngineSequence = 
            //    GameState.ActiveMorphid.EngineSequence.Concat(SequenceAddition).ToArray();
        }
    }

    public  int Targeting () {
        return 0;
    }

    public  TargetingType TargetingType () {
        return global::TargetingType.Skip;
    }
}