using _Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Segment : MonoBehaviour
{
    public bool IsOn { get; private set; }
    public bool IsClickable = true;

    [SerializeField] SpriteRenderer[] SegmentParts;
    [SerializeField] SegmentsScriptableObject SegmentSO;

    Color oldColor;
    float alpha, oldAlpha;

    private void FixedUpdate()
    {
        if (oldColor != SegmentSO.SegmentColor || oldAlpha != alpha)
        {
            oldAlpha = Mathf.Lerp(oldAlpha, alpha, SegmentSO.AlphaFading);
            oldColor = oldColor.Lerp(SegmentSO.SegmentColor, SegmentSO.ColorFading);
            oldColor.a = oldAlpha;
            SegmentParts.Fade(oldAlpha, oldColor);
        }
    }

    public void TurnOn()
    {
        alpha = 1.0f;
        IsOn = true;
    }
    public void TurnOff()
    {
        alpha = SegmentSO.AlphaMin;
        IsOn = false;
    }
    void OnMouseEnter() 
    {
        if (IsClickable) alpha = IsOn ? 0.5f : 0.1f;
    }
    void OnMouseExit()
    {
        if (IsClickable)
        {
            if (IsOn) TurnOn();
            else TurnOff();
        }
    }
    void OnMouseDown()
    {
        if (IsClickable)
        {
            if (!IsOn) TurnOn();
            else TurnOff();
        }
    }

    private void OnDestroy()
    {
        foreach (var part in SegmentParts) Destroy(part);
    }
}
