using UnityEngine;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour {
    protected static GameBoard Singleton;

    public void Awake() {
        Singleton = this;
        Hide();
    }
    
    public static void Show() {
        Singleton.gameObject.SetActive(true);
    }
    
    public static void Hide() {
        Singleton.gameObject.SetActive(false);
    }
}