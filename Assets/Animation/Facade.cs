using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProtoBuf;
using ProtoBuf.Meta;

public delegate object Truth(params object[] args);

public enum Facet {
    MinionAlive,
    MinionHealth,
    MorphidHealth,
    MyTurn,
}

public static class TruthFunctions {
    public static object MinionAlive(params object[] args) {
        return GameState.GetMinion((string)args[0]) != null;
    }
    
    public static object MinionHealth(params object[] args) {
        return GameState.GetMinion((string)args[0]).Defense;
    }
    
    public static object MorphidHealth(params object[] args) {
        return GameState.GetMorphid((string)args[0]).Health;
    }
    
    public static object MyTurn(params object[] args) {
        return GameState.IsLocalActive;
    }

    public static Truth GetTruth(this Facet e) {
        switch (e) {
        case Facet.MinionAlive:
            return MinionAlive;
        case Facet.MinionHealth:
            return MinionHealth;
        case Facet.MorphidHealth:
            return MorphidHealth;
        case Facet.MyTurn:
            return MyTurn;
        }
        Debug.Log("No truth function assigned to enum: " + e.ToString());
        return null;
    }
}

public class Facade {
    public static Facade Singleton;

    public Dictionary<Facet, HashSet<Lie>> Lies;

    static Facade() {
        Singleton = new Facade();
        Singleton.Lies = new Dictionary<Facet, HashSet<Lie>>();
        foreach (Facet f in Enum.GetValues(typeof(Facet)).Cast<Facet>()) {
            Singleton.Lies[f] = new HashSet<Lie>();
        }
    }
    
    public static void AddLie(Lie l) {
        Singleton.Lies[l.Facet].Add(l);
    }
    
    public static void RemoveLie(Lie l) {
        Singleton.Lies[l.Facet].Remove(l);
    }

    public static object Query(Facet f,  params object[] args) {
        foreach (Lie l in Singleton.Lies[f]) {
            if (l.Applies(args)) {
                return l.TellTale();
            }
        }
        return f.GetTruth()(args);
    }
}