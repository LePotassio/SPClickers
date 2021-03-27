using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticGlobals : MonoBehaviour
{
    //public static GameManager gm;
    //public static SingletonPrefabs sp;

    /*
    static StaticGlobals()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        sp = GameObject.FindObjectOfType<SingletonPrefabs>();
    }*/

    public static Transform FindChildWithTag(Transform transform, string tag)
    {
        foreach (Transform child in transform)
        {
            if (child.tag == tag)
                return child;
        }
        return null;
    }
}
