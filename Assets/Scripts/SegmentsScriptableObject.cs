using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "SegmentsScriptableObject", menuName = "ScriptableObjects/Segments")]
public class SegmentsScriptableObject : ScriptableObject
{
    [Tooltip("The color of the segments")]
    public Color SegmentColor;
    [Tooltip("How fast the segment's turn on and off")]
    public float AlphaFading;
    [Tooltip("How fast the segment's color changes")]
    public float ColorFading;
    [Tooltip("Lowest alpha value the segment will turn off to")]
    public float AlphaMin = 0;
}
