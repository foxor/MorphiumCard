using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Assets.Card.Effects;

public class CardEffectTable {
    public static string NamespacedAssemblyNameTemplate = typeof(ExoTruck).AssemblyQualifiedName.Replace("ExoTruck", "*");
    public static string GlobalAssemblyNameTemplate = typeof(Consolodate).AssemblyQualifiedName.Replace("Consolodate", "*");

    public static Effect Map(string name, string text) {
        name = name.Replace(" ", "");
        Type t = Type.GetType(NamespacedAssemblyNameTemplate.Replace("*", name));
        if (t == null) {
            t = Type.GetType(GlobalAssemblyNameTemplate.Replace("*", name));
            if (t == null) {
                return null;
            }
            else {
                return (Effect)Activator.CreateInstance(t, text);
            }
        }
        else {
            return (Effect)Activator.CreateInstance(t, text);
        }
    }
}