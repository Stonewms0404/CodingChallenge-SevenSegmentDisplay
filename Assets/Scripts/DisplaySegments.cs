using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class DisplaySegments : MonoBehaviour
{
    public static event Action<GameObject> DoneDisplayingSegments;
    public bool IsSegmentClickable
    {
        set { foreach (Segment segment in segments) segment.IsClickable = value; }
        get { return IsSegmentClickable; }
    }

    [SerializeField] GameObject segmentObj;
    [SerializeField] Vector3[] SegmentPoints;

    public Segment[] segments;

    private void Awake()
    {
        List<Segment> list = new();
        for (int i = 0; i < 7; i++) DrawSegment(i, list);
        segments = list.ToArray();

    }
    private void Start()
    {
        DoneDisplayingSegments(this.gameObject);
    }
    public void DrawSegment(int iter, List<Segment> tempList)
    {
        segmentObj = Instantiate(
        segmentObj,
        new Vector3(SegmentPoints[iter].x, SegmentPoints[iter].y, 0.0f) + transform.position,
        Quaternion.Euler(0.0f, 0.0f, SegmentPoints[iter].z),
        this.transform);

        tempList.Add(segmentObj.GetComponent<Segment>());
    }

    public void Zero()
    {
        int i = 0;
        foreach (Segment segment in segments)
        {
            if (i == 2)  segment.TurnOff();
            else segment.TurnOn();
            i++;
        }
    }
    public void One()
    {
        int i = 0;
        foreach (Segment segment in segments)
        {
            if (i == 4 || i == 5) segment.TurnOn();
            else segment.TurnOff();
            i++;
        }
    }
    public void Two()
    {
        int i = 0;
        foreach (Segment segment in segments)
        {
            if (i <= 4) segment.TurnOn();
            else segment.TurnOff();
            i++;
        }
    }
    public void Three()
    {
        int i = 0;
        foreach (Segment segment in segments)
        {
            if (i == 3 || i == 6) segment.TurnOff();
            else segment.TurnOn();
            i++;
        }
    }
    public void Four()
    {
        int i = 0;
        foreach (Segment segment in segments)
        {
            if (i == 0 || i == 1 || i == 3) segment.TurnOff();
            else segment.TurnOn();
            i++;
        }
    }
    public void Five()
    {
        int i = 0;
        foreach (Segment segment in segments)
        {
            if (i == 4 || i == 3) segment.TurnOff();
            else segment.TurnOn();
            i++;
        }
    }
    public void Six()
    {
        int i = 0;
        foreach (Segment segment in segments)
        {
            if (i == 4) segment.TurnOff();
            else segment.TurnOn();
            i++;
        }
    }
    public void Seven()
    {
        int i = 0;
        foreach (Segment segment in segments)
        {
            if (i == 5 || i == 4 || i == 1) segment.TurnOn();
            else segment.TurnOff();
            i++;
        }
    }
    public void Eight()
    {
        foreach (Segment segment in segments) segment.TurnOn();
    }
    public void Nine()
    {
        int i = 0;
        foreach (Segment segment in segments)
        {
            if (i == 0 || i == 3) segment.TurnOff();
            else segment.TurnOn();
            i++;
        }
    }

    private void OnDestroy()
    {
        foreach (Segment segment in segments) Destroy(segment);
    }
}