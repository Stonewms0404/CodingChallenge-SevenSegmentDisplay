using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHover : MonoBehaviour
{
    public static event Action<bool> HoveredPanel;
    private void OnMouseEnter() => HoveredPanel(false);
    private void OnMouseExit() => HoveredPanel(true);
}
