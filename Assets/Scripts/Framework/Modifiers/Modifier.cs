using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier<TModifiable>
{
    private int _key;

    public int Key
    {
        get
        {
            return (int)this._key;
        }

        set
        {
            this._key = value;
        }
    }

    public Modifier()
    {

    }
}
