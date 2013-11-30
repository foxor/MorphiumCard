using UnityEngine;
using System.Collections;

public class LetterboxClamp : MonoBehaviour {

    protected const int ART_HEIGHT = 1080;
    protected const float ART_ASPECT = 16f / 9f;
    protected const float ASPECT_RATIO_EPSILON = 1e-5f;
    protected int Width;
    protected int Height;

    public void Start () {
        Clamp();
    }

    protected void Clamp () {
        Width = Screen.width;
        Height = Screen.height;
        float ScreenAspect = ((float)Width) / ((float)Height);
        if (ScreenAspect + ASPECT_RATIO_EPSILON < ART_ASPECT) {
            Camera.main.orthographicSize = (int)(((float)ART_HEIGHT) * (ART_ASPECT / ScreenAspect));
        } else {
            Camera.main.orthographicSize = ART_HEIGHT;
        }
    }

    void Update () {
        if (Screen.width != Width || Screen.height != Height) {
            Clamp();
        }
    }
}