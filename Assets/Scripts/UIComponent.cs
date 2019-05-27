using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponent : MonoBehaviour {

    [SerializeField]
    string ComponentName = "";

    public string GetName()
    {
        return ComponentName;
    }
}
