using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

public enum AnimationType {
    AttackMinion,
    AttackMorphid,
}

public abstract class SignalData {
    public virtual void OnQueue() {}
    public virtual void OnActivate() {}
    public virtual void OnDequeue() {}

    public virtual bool IsActive() {
        return false;
    }
}

public class AnimationSignalManager : MonoBehaviour {

    protected Queue<SignalData> Signals = new Queue<SignalData>();
    
    [RPC]
    public void QueueMinionAnimation (string data) {
        Queue<MinionAnimation>(data);
    }
    
    [RPC]
    public void QueueMinionHealthLie (string data) {
        Queue<MinionHealthLie>(data);
    }
    
    [RPC]
    public void QueueMinionAliveLie (string data) {
        Queue<MinionAliveLie>(data);
    }

    protected void Queue<T>(string data) where T : SignalData {
        T signal = data.DeserializeProtoString<T>();
        signal.OnQueue();
        if (!Signals.Any()) {
            signal.OnActivate();
        }
        Signals.Enqueue(signal);
    }
    
    public void Update() {
        while (Signals.Any() && !Signals.Peek().IsActive()) {
            Signals.Dequeue().OnDequeue();
            if (Signals.Any()) {
                Signals.Peek().OnActivate();
            }
        }
    }
}