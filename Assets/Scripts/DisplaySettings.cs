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

    [Header("Time")]
    [SerializeField] GameObject TimePanel;

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
        bool isClickable = false;
        switch (displayStateStr.State)
        {
            case DisplayState.NumberEnter: NumberEnter(out isClickable);  break;
            case DisplayState.Time: DisplayTime(out isClickable);                  break;
            case DisplayState.Incrementer: Incrementer(out isClickable);     break;
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Colon")) Destroy(obj);

        if (CurrentModeText)
            CurrentModeText.text = displayStateStr.Text;
        StateChanged(displayStateStr.State);
    }

    void NumberEnter(out bool clickable)
    {
        NumberEnterPanel.SetActive(true);
        TimePanel.SetActive(false);

        clickable = false;
    }
    void DisplayTime(out bool clickable)
    {
        NumberEnterPanel.SetActive(false);
        TimePanel.SetActive(true);

        clickable = false;
    }
    void Incrementer(out bool clickable)
    {
        NumberEnterPanel.SetActive(false);
        TimePanel.SetActive(false);

        clickable = false;
    }
}
