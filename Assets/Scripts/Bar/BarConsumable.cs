using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarConsumable
{
    public enum Type
    {
        Beer,
        Cake,
        Talk
    }

    private Type type;

    public BarConsumable(Type type)
    {
        this.type = type;
    }

    public Type GetType()
    {
        return type;
    }
}
