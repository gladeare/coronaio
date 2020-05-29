using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource_Bar_Color : MonoBehaviour
{
    [SerializeField]
    private Image fill;
    [SerializeField]
    public Color fullColor;
    [SerializeField]
    public Color emptyColor;
    private float fillAmountFloat;

    void Update()
    {
        fillAmountFloat = fill.fillAmount; //converting fillAmount into a float so the lerp works
        fill.color = Color.Lerp(emptyColor, fullColor, fillAmountFloat);
    }

}