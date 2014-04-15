using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public delegate int DynamicProvider();

public abstract class Effect {
    protected TextProvider textProvider;
    public string Text
    {
        get
        {
            return textProvider();
        }
    }

    protected int targetTypeFlags;
    public int TargetFlags
    {
        get
        {
            return targetTypeFlags;
        }
    }

    public Effect(string text)
    {
        textProvider = Templating.Template(text, TemplatingArguments().ToArray());
        foreach (TargetTypeFlag ttf in TargetTypeFlags())
        {
            targetTypeFlags |= (int)ttf;
        }
    }

    protected abstract IEnumerable<DynamicProvider> TemplatingArguments();
    protected abstract IEnumerable<TargetTypeFlag> TargetTypeFlags();

    public abstract void Apply(string guid);
    public abstract int Cost();
    public abstract TargetingType TargetingType();

    public virtual bool TargetScanner(string guid) {
        return true;
    }
    public virtual void GlobalApply() {}

    protected string Name {
        get {
            return this.GetType().Name;
        }
    }
}