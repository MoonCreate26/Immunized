using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchTargetSprite : MonoBehaviour
{
    public UniversalPathogenDictionary pathogenDictionary;
    
    public Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void UpdateSprite(int idx)
    {
        image.sprite = (Sprite)pathogenDictionary.GetPathogenInfo(idx, "Sprite");
    }
}
