using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace _Scripts
{
    /// <summary>
    /// Extension methods for certain methods that are not available to me.
    /// </summary>
    public static class ExtraMethods
    {
        /// <summary>
        /// Sets the alpha value of the SpriteRenderer's color to the new alpha value passed in.
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <param name="alpha">New alpha value for the color</param>
        public static void Fade(this SpriteRenderer spriteRenderer, float alpha)
        {
            var color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
        /// <summary>
        /// Sets the alpha value of the SpriteRenderer's color to the new alpha and color value passed in.
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <param name="alpha">New alpha value for the color</param>
        public static void Fade(this SpriteRenderer spriteRenderer, float alpha, Color color)
        {
            color.a = alpha;
            spriteRenderer.color = color;
        }
        /// <summary>
        /// Sets the alpha value of the SpriteRenderer's color to the new alpha value passed in. (Using an array)
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <param name="alpha">New alpha value for the color</param>
        public static void Fade(this SpriteRenderer[] spriteRenderer, float alpha)
        {
            foreach (var sprite in spriteRenderer) sprite.Fade(alpha);
        }
        /// <summary>
        /// Sets the alpha value of the SpriteRenderer's color to the new alpha and color value passed in. (Using an array)
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <param name="alpha">New alpha value for the color</param>
        public static void Fade(this SpriteRenderer[] spriteRenderer, float alpha, Color color)
        {
            foreach (var sprite in spriteRenderer) sprite.Fade(alpha, color);
        }

        /// <summary>
        /// Takes the last item of the list and removes it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Pop<T>(this List<T> list)
        {
            list.Remove(list.Last());
        }
        /// <summary>
        /// Checks if the list is null. If it is, create a new list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Check<T>(this List<T> list)
        {
            if (list == null) list = new List<T>();
        }

        /// <summary>
        /// Changes the boolean to an integer. true: 1. false: 0.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this bool  value)
        {
            return value ? 1 : 0;
        }

        public static Color Lerp(this Color c1, Color c2, float amount)
        {
            c1.r = Mathf.Lerp(c1.r, c2.r, amount);
            c1.g = Mathf.Lerp(c1.g, c2.g, amount);
            c1.b = Mathf.Lerp(c1.b, c2.b, amount);
            return c1;
        }

    }
}

public struct DisplayStateStruct
{
    public DisplayState State { get; private set; }
    public readonly string Text {
        get
        {
            return "Current Mode:\n" + State switch
            {
                //DisplayState.Clickable => "Clickable",
                DisplayState.NumberEnter => "Number Enter",
                DisplayState.Time => "Current Time",
                DisplayState.Incrementer => "Incrementer",
                _ => ""
            };
        }
    }

    public DisplayStateStruct(DisplayState newState) => State = newState;

    public static DisplayStateStruct operator ++(DisplayStateStruct DSS)
    {
        DSS.State = DSS.State switch
        {
            //DisplayState.Clickable => ,
            DisplayState.NumberEnter => DisplayState.Time,
            DisplayState.Time => DisplayState.Incrementer,
            DisplayState.Incrementer => DisplayState.NumberEnter,
            _ => DisplayState.NumberEnter
        };
        return DSS;
    }
    public static DisplayStateStruct operator --(DisplayStateStruct DSS)
    {
        DSS.State = DSS.State switch
        {
            //DisplayState.Clickable => ,
            DisplayState.NumberEnter => DisplayState.Incrementer,
            DisplayState.Time => DisplayState.NumberEnter,
            DisplayState.Incrementer => DisplayState.Time,
            _ => DisplayState.NumberEnter
        };
        return DSS;
    }

}
public struct TimeStruct
{
    public TimeStruct(int hour, int min, int sec)
    {
        ColonObjs = new GameObject[2];
        HourSeg = new DisplaySegments[2];
        MinSeg = new DisplaySegments[2];
        SecSeg = new DisplaySegments[2];
        Hour = hour;
        Min = min;
        Sec = sec;
    }
    public void Clear()
    {
        ColonObjs.ToList().Clear(); ColonObjs = null;
        HourSeg.ToList().Clear(); HourSeg = null;
        MinSeg.ToList().Clear(); MinSeg = null;
        SecSeg.ToList().Clear(); SecSeg = null;
    }

    public void DisplayTime()
    {
        StartObject.GetNumber(Hour / 10, HourSeg[0]);
        StartObject.GetNumber(Hour % 10, HourSeg[1]);
        StartObject.GetNumber(Min / 10, MinSeg[0]);
        StartObject.GetNumber(Min % 10, MinSeg[1]);
        StartObject.GetNumber(Sec / 10, SecSeg[0]);
        StartObject.GetNumber(Sec % 10, SecSeg[1]);
    }

    public int Hour;
    public int Min;
    public int Sec;
    public GameObject[] ColonObjs;
    public DisplaySegments[] HourSeg;
    public DisplaySegments[] MinSeg;
    public DisplaySegments[] SecSeg;
}

public enum DisplayState
{
    //Clickable,
    NumberEnter,
    Time,
    Incrementer
}
