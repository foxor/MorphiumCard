using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Templating {
    public static char[] SUBSTITUTION_CHAR = new char[]{'*'};

    public static IEnumerable<string> JoinStrings(string[] parts, DynamicProvider[] providers) {
        for (int i = 0; i < parts.Length || i < providers.Length; i++) {
            if (i < parts.Length) {
                yield return parts[i];
            }
            if (i < providers.Length) {
                yield return providers[i]().ToString();
            }
        }
    }

    public static TextFn Template(string original, params DynamicProvider[] providers) {
        string[] parts = original.Split(SUBSTITUTION_CHAR);
        return () => {
            return string.Join("", JoinStrings(parts, providers).ToArray());
        };
    }
}