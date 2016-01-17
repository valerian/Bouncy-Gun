using UnityEngine;
using System.Collections;

public abstract class Bonus
{
    public int value = 1;
    public string verboseName { get; protected set; }
    public bool isMalus { get { return value < 0; } }

    abstract public string GetDescriptionText();
    abstract public string GetValueText();
    abstract public void Apply();

    public Bonus Clone()
    {
        return (Bonus) this.MemberwiseClone();
    }
}

