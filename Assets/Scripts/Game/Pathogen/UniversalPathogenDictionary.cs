using System;
using System.Collections.Generic;
using UnityEngine;

public class UniversalPathogenDictionary : MonoBehaviour
{
    [UDictionary.Split(30, 70)]
    public UDictionary1 pathogenDictionary;
    [Serializable]
    public class UDictionary1 : UDictionary<int, PathogenInfo> { }

    [Serializable]
    public class Key
    {
        public string id;

        public string file;
    }

    [Serializable]
    public class Value
    {
        public string firstName;

        public string lastName;
    }

    public object GetPathogenInfo(int idx, string operationType)
    {
        if(operationType == "Sprite")
        {
            return pathogenDictionary[idx].Texture;
        }

        else if(operationType == "Color")
        {
            return pathogenDictionary[idx].AntibodyColor;
        }

        else if(operationType == "Delay")
        {
            return pathogenDictionary[idx].SpawnDelay;
        }

        else
        {
            Debug.LogError("Invalid Operation for 'GetPathogenInfo' within UniversalPathogen Dictionary! Only 'Sprite' or 'Color' is accepted.");
            return "Error!";
        }
    }
}

[System.Serializable]
public class PathogenInfo
{
    public Color AntibodyColor;
    public Sprite Texture;
    public float SpawnDelay;
}

