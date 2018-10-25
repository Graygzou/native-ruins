using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class MinMaxSliderAttribute : PropertyAttribute
{
    public int min;
    public int max;

    public MinMaxSliderAttribute(int v1, int v2)
    {
        this.min = v1;
        this.max = v2;
    }
}