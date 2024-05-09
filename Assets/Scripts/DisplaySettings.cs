using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplaySettings : MonoBehaviour
{
    public static event Action<DisplayState> StateChanged;

    [Header("Number Enter")]
    [SerializeField] GameObject NumberEnterPanel;

    [Header("Other/Settings")]
    [SerializeField] TextMeshProUGUI CurrentModeText;
    [SerializeField] DisplayState dispState;

    DisplayStateStruct displayStateStr;

    void Start()
    {
        displayStateStr = new DisplayStateStruct(dispState);

        ChangeDisplayState();
    }

    public void CycleDisplayStatesPlus()
    {
        displayStateStr++;
        ChangeDisplayState();
    }
    public void CycleDisplayStatesMinus()
    {
        displayStateStr--;
        ChangeDisplayState();
    }

    void ChangeDisplayState()
    {
        switch (displayStateStr.State)
        {
            case DisplayState.NumberEnter: NumberEnterPanel.SetActive(true);           break;
            case DisplayState.Time:                NumberEnterPanel.SetActive(false);         break;
            case DisplayState.Incrementer:    NumberEnterPanel.SetActive(false);         break;
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Colon")) Destroy(obj);

        if (CurrentModeText)
            CurrentModeText.text = displayStateStr.Text;
        StateChanged(displayStateStr.State);
    }
}
