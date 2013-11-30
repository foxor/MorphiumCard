using UnityEngine;
using System.Collections.Generic;

public delegate void Callback (object data);

// I would add "where ENUM_TYPE : enum" here, but it's not supported by C#
public interface IEventListener<E> {
    void AddCallback (E trigger, Callback callback);

    bool RemoveCallback (E trigger, Callback callback);

    void Broadcast (E trigger, object data);
}

public class EventListener<E> : IEventListener<E> {
    protected Dictionary<E, List<Callback>> callbacks;
    
    public EventListener () {
        callbacks = new Dictionary<E, List<Callback>> ();
    }
    
    public void AddCallback (E trigger, Callback callback) {
        List<Callback> registered;
        if (callbacks.ContainsKey(trigger)) {
            registered = callbacks[trigger];
        } else {
            registered = new List<Callback> ();
            callbacks[trigger] = registered;
        }
        registered.Add(callback);
    }

    public bool RemoveCallback (E trigger, Callback callback) {
        List<Callback> registered = callbacks[trigger];
        return registered != null && registered.Remove(callback);
    }

    public void Broadcast (E trigger, object data) {
        if (callbacks.ContainsKey(trigger)) {
            List<Callback> registered = callbacks[trigger];
            if (registered != null) {
                foreach (Callback callback in registered.ToArray()) {
                    callback(data);
                }
            }
        }
    }
}