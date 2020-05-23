using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bar : MonoBehaviour
{
    private Transform bar;
    private TextMeshPro text;

    void Awake()
    {
        bar = transform.Find("Bar");
        text = bar.parent.Find("Value").GetComponent<TextMeshPro>();
    }

    public void SetSize(float normalizedValue)
    {
        normalizedValue = normalizedValue >= 0 ? normalizedValue : 0;
        bar.localScale = new Vector2(normalizedValue, 1);
    }

    public void SetValue(string value)
    {
        text.text = value;
    }

    public void SwitchValue(bool on)
    {
        text.gameObject.SetActive(on);
    }
}