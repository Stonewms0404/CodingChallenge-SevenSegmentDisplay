using _Scripts;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartObject : MonoBehaviour
{
    /// <summary>
    /// Changes the object's scale depending on the scale factor.
    /// </summary>
    public Action<GameObject> ChangeScale;
    /// <summary>
    /// Changes the scale factor depending on the amount of displays needed.
    /// </summary>
    public Action<int> SetScaleFactor;
    /// <summary>
    /// Creates the DisplaySegments object using a Func and returns the object created.
    /// </summary>
    public Func<DisplaySegments> MakeDisplay;

    [SerializeField] DisplaySegments display;
    [SerializeField] GameObject colonObj;
    [SerializeField] InputAction UpInput;
    [SerializeField] InputAction DownInput;
    [SerializeField] SegmentsScriptableObject SegmentsSO;

    GameObject displayContainer, timeContainer;
    List<DisplaySegments> displays;
    GameObject[] timeObjects;
    TimeStruct timeStruct;

    string num;
    float scaleFactor;
    bool usingTime = false, canIncrement;
    int numOfDisplays = 0, incrementer;
    const float paddingRL = 5.0f, paddingTB = 9.0f;

    private void EnableInputs()
    {
        UpInput.Enable();
        UpInput.performed += _ => OnIncrementerUp();
        DownInput.Enable();
        DownInput.performed += _ => OnIncrementerDown();
    }

    private void DisableInputs()
    {
        UpInput.Disable();
        UpInput.performed -= _ => OnIncrementerUp();
        DownInput.Disable();
        DownInput.performed -= _ => OnIncrementerDown();
    }
    private void OnDestroy()
    {
        DisableInputs();
        PanelHover.HoveredPanel -= OnPanelHovered;
        DisplaySegments.DoneDisplayingSegments -= ChangeScale;
        DisplaySettings.StateChanged -= state => {
            incrementer = 0;
            ChangeAmountOfSegments(0);
            usingTime = (state == DisplayState.Time);
            if (state == DisplayState.Incrementer) EnableInputs();
            else DisableInputs();

            if (displays != null) foreach (DisplaySegments display in displays) Destroy(display);
            if (timeObjects != null) foreach (GameObject timeObject in timeObjects) Destroy(timeObject);
            displays = null;
            timeObjects = null;
            if (timeStruct.ColonObjs != null)
                timeStruct.Clear();
        };
    }

    void Awake()
    {
        ChangeScale = obj => obj.transform.localScale = Vector3.one * scaleFactor;
        SetScaleFactor = amount => scaleFactor = ((amount > 1 && amount <= 20).ToInt() * (2.0f / amount)) + ((amount > 20).ToInt() * (2.0f / 20.0f) + ((amount == 1).ToInt() * 1.0f));
        MakeDisplay = () => Instantiate(display, displayContainer.transform);

        PanelHover.HoveredPanel += OnPanelHovered;
        DisplaySegments.DoneDisplayingSegments += ChangeScale;
        DisplaySettings.StateChanged += state => {
            incrementer = 0;
            ChangeAmountOfSegments(0);
            usingTime = (state == DisplayState.Time);
            if (state == DisplayState.Incrementer) EnableInputs();
            else DisableInputs();

            if (displays != null) foreach (DisplaySegments display in displays) Destroy(display);
            if (timeObjects != null) foreach (GameObject timeObject in timeObjects) Destroy(timeObject);
            displays = null;
            timeObjects = null;
            if (timeStruct.ColonObjs != null)
                timeStruct.Clear();
        };

        displayContainer = Instantiate(new GameObject());
        displayContainer.name = "DisplayContainer";

        timeContainer = Instantiate(new GameObject());
        timeContainer.name = "TimeContainer";
    }

    void OnPanelHovered (bool value)
    {
        canIncrement = value;
    }

    private void Update()
    {
        if (usingTime)
        {
            if (timeStruct.ColonObjs == null)
            {
                ChangeDisplayForTime();
                AdjustDisplayForTime();
            }
            DisplayTime(DateTime.Now);
        }
    }

    void OnIncrementerUp()
    {
        if (canIncrement)
        {
            string number = "" + ++incrementer;
            ChangeAmountOfSegments(number.Length);

            int i = 0;
            foreach (DisplaySegments dis in displays) GetNumber(CheckIfCharIsInt(number[i++]), dis);
        }
    }
    private void OnIncrementerDown()
    {
        if (canIncrement)
        {
            string number = "" + --incrementer;
            ChangeAmountOfSegments(number.Length);

            int i = 0;
            foreach (DisplaySegments dis in displays) GetNumber(CheckIfCharIsInt(number[i++]), dis);
        }
    }

    public void ChangeAmountOfSegments(float amount)
    {
        if (displays != null)
        { 
            foreach (var dis in displays) Destroy(dis.gameObject);
            displays.Clear(); 
        }
        else displays = new List<DisplaySegments>();
        for (int i = 0; i < amount; i++)
            displays.Add(Instantiate(display, displayContainer.transform));

        UpdateSegments((int)amount);
    }
    void UpdateSegments(int amount)
    {
        if (amount == 0 && displays != null)
        {
            foreach (var display in displays)
                Destroy(display.gameObject);
            displays.Clear();
        }

        numOfDisplays = amount;

        SetScaleFactor(numOfDisplays);
        AdjustDisplays();
    }
    void AdjustDisplays()
    {
        int i = 0, k = 0;
        float adjustY = 0.0f, adjustX = 0.0f;
        Vector3 objPos = Vector3.zero;
        bool HasAnotherLine = false;
        displays.Check();
        if (displays != null) foreach (DisplaySegments dis in displays)
        {
            if (i > 0)
            {
                Vector3 dispPos = dis.gameObject.transform.position;
                if (i / 20 == 1)
                {
                    adjustY = dispPos.y - (paddingTB * scaleFactor * ++k);
                    HasAnotherLine = true;
                    i = 0;
                }

                adjustX = dispPos.x + ((paddingRL * i) * scaleFactor);

                dis.gameObject.transform.position = new(adjustX, adjustY, dispPos.z);

                if (!HasAnotherLine)
                    objPos.x = paddingRL * scaleFactor / 2 - paddingRL;
                displayContainer.transform.position = objPos;
                ChangeScale(dis.gameObject);
            }
            i++;
        }
        
        if (numOfDisplays == 1)
            displayContainer.transform.position = Vector3.zero;

        if (numOfDisplays / 20 >= 1)
            objPos.y = paddingTB * scaleFactor / 2 * (int)(numOfDisplays / 20);
        displayContainer.transform.position = objPos;
        if (numOfDisplays > 1 && displays != null)
        {
            DisplaySegments[] tempDisplays = displays.ToArray();
            tempDisplays[1].transform.position = new(tempDisplays[1].transform.position.x, tempDisplays[0].transform.position.y, tempDisplays[1].transform.position.z);
        }
    }
    
    void DisplayTime(DateTime dateTime)
    {
        timeStruct.Hour = dateTime.Hour;
        timeStruct.Min = dateTime.Minute;
        timeStruct.Sec = dateTime.Second;
        timeStruct.DisplayTime();
    }
    void ChangeDisplayForTime()
    {
        if (displays != null)
        {
            foreach (var dis in displays) Destroy(dis.gameObject);
            displays.Clear();
        }
        else displays = new List<DisplaySegments>();

        if (timeStruct.ColonObjs != null)
            timeStruct.Clear();

        timeStruct = new TimeStruct(0, 0, 0);
        timeStruct.HourSeg[0] = MakeDisplay();
        timeStruct.HourSeg[1] = MakeDisplay();
        timeStruct.MinSeg[0] = MakeDisplay();
        timeStruct.MinSeg[1] = MakeDisplay();
        timeStruct.SecSeg[0] = MakeDisplay();
        timeStruct.SecSeg[1] = MakeDisplay();
        GameObject tempObj = Instantiate(colonObj, displayContainer.transform);
        timeStruct.ColonObjs[0] = tempObj;
        tempObj = Instantiate(colonObj, displayContainer.transform);
        timeStruct.ColonObjs[1] = tempObj;
    }
    void AdjustDisplayForTime()
    {
        int i = 0;
        Vector3 objPos = Vector3.zero;
        timeObjects = new GameObject[8] {
            timeStruct.HourSeg[0].gameObject,
            timeStruct.HourSeg[1].gameObject,
            timeStruct.ColonObjs[0],
            timeStruct.MinSeg[0].gameObject,
            timeStruct.MinSeg[1].gameObject,
            timeStruct.ColonObjs[1],
            timeStruct.SecSeg[0].gameObject,
            timeStruct.SecSeg[1].gameObject
        };
        SetScaleFactor(timeObjects.Length);

        foreach (GameObject obj in timeObjects)
        {
            if (i > 0)
            {
                Vector3 objePos = obj.transform.position;

                float adjustX = objePos.x + ((paddingRL * i) * scaleFactor);

                obj.transform.position = new(adjustX, 0, objePos.z);

                objePos.x = paddingRL * scaleFactor / 2 - paddingRL;
                timeContainer.transform.position = objePos;
                ChangeScale(obj);
            }
            i++;
        }
        
        if (numOfDisplays == 1)
            displayContainer.transform.position = Vector3.zero;

        objPos.x = paddingRL * scaleFactor / 2 - paddingRL;

        displayContainer.transform.position = objPos;
        ChangeScale(gameObject);
    }

    public void Input(string input)
    {
        num = "";
        for (int i = 0; i < input.Length; i++)
        {
            int j = CheckIfCharIsInt(input[i]);
            if (j != 10)
                num += j;
        }
        ChangeAmountOfSegments(num.Length);
        ChangeDisplayNum();
    }
    void ChangeDisplayNum()
    {
        int i = 0;
        foreach (DisplaySegments disp in displays) GetNumber(CheckIfCharIsInt(num[i++]), disp);
    }
    public static int CheckIfCharIsInt(char ch)
    {
        return (int)ch switch
        {
            48 => 0,
            49 => 1,
            50 => 2,
            51 => 3,
            52 => 4,
            53 => 5,
            54 => 6,
            55 => 7,
            56 => 8,
            57 => 9,
            _ => 10,
        };
    }
    public static void GetNumber(int num, DisplaySegments dis)
    {
        switch (num)
        {
            case 0: dis.Zero(); return;
            case 1: dis.One(); return;
            case 2: dis.Two(); return;
            case 3: dis.Three(); return;
            case 4: dis.Four(); return;
            case 5: dis.Five(); return;
            case 6: dis.Six(); return;
            case 7: dis.Seven(); return;
            case 8: dis.Eight(); return;
            case 9: dis.Nine(); return;
        }
    }
}