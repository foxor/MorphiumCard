using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Assets.Card.Effects;

public class CardEffectTable {
	public static string TemplateAssemblyName = typeof(ExoTruck).AssemblyQualifiedName.Replace("ExoTruck", "*");

    public static Effect Map(string name, string text) {
        name = name.Replace(" ", "");
		Type t = Type.GetType(TemplateAssemblyName.Replace("*", name));
        return (Effect)Activator.CreateInstance(t, text);
    }
}