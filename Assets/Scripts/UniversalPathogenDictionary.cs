using System;
using System.Collections.Generic;
using UnityEngine;

public class UniversalPathogenDictionary : MonoBehaviour
{
    [UDictionary.Split(30, 70)]
    public UDictionary1 pathogenDictionary;
    [Serializable]
    public class UDictionary1 : UDictionary<string, PathogenInfo> { }

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

    public object GetPathogenInfo(string pathogenName, string operationType)
    {
        if(operationType == "Sprite")
        {
            return pathogenDictionary[pathogenName].Texture;
        }

        else if(operationType == "Color")
        {
            return pathogenDictionary[pathogenName].AntibodyColor;
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
}

