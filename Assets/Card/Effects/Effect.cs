using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public delegate void ApplySingleFn(string guid);
public delegate void ApplyFn(IEnumerable<string> guids);
public delegate int CostFn();
public delegate string TextFn();

public class Effect {
    public Effect (ApplyFn apply, CostFn cost, TextFn text, int targeting, TargetingType targetingType) {
        this.Apply = apply;
        this.Cost = cost;
        this.Text = text;
        this.Targeting = targeting;
        this.TargetingType = targetingType;
    }

    public ApplyFn Apply;
    public CostFn Cost;
    public TextFn Text;
    public int Targeting;
    public TargetingType TargetingType;
}