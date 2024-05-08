using _Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class Colon : MonoBehaviour
{
    Color oldColor;
    float oldAlpha, alpha, fadeTimerToReach = 0.5f, timer = 0;
    bool isOn;

    [SerializeField] SpriteRenderer[] ColonParts;
    [SerializeField] SegmentsScriptableObject SegmentSO;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer < fadeTimerToReach)
            timer += Time.deltaTime;
        else
        {
            if (isOn) TurnOff();
            else TurnOn();
            timer = 0;
        }

        if (oldColor != SegmentSO.SegmentColor || oldAlpha != alpha)
        {
            oldAlpha = Mathf.Lerp(oldAlpha, alpha, SegmentSO.AlphaFading);
            oldColor = oldColor.Lerp(SegmentSO.SegmentColor, SegmentSO.ColorFading);
            oldColor.a = oldAlpha;
            ColonParts.Fade(oldAlpha, oldColor);
        }
    }
    public void TurnOn()
    {
        alpha = 1.0f;
        isOn = true;
    }
    public void TurnOff()
    {
        alpha = SegmentSO.AlphaMin;
        isOn = false;
    }
}
