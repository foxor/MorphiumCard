using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public delegate string TextProvider();

public static class Templating {
    public static char[] SUBSTITUTION_CHAR = new char[]{'*', '-', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};

    public static IEnumerable<string> JoinStrings(string[] parts, DynamicProvider[] providers) {
        if (providers.Length + 1 != parts.Length) {
            Debug.Log("Incorrect parameter count for card with text: " + string.Join("", parts));
        }
        for (int i = 0; i < parts.Length || i < providers.Length; i++) {
            if (i < parts.Length) {
                yield return parts[i];
            }
            if (i < providers.Length) {
                yield return providers[i]().ToString();
            }
        }
    }

    public static TextProvider Template(string original, params DynamicProvider[] providers)
    {
        string[] parts = original.Split(SUBSTITUTION_CHAR, StringSplitOptions.RemoveEmptyEntries);
        return () => {
            return string.Join("", JoinStrings(parts, providers).ToArray());
        };
    }
}