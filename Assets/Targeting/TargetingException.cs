using UnityEngine;
using System;
using System.Collections;

public class TargetingException : Exception {
    public TargetingException(string message) : base(message) {}
}