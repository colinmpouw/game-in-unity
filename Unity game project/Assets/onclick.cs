using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onclick : MonoBehaviour
{
    [SerializeField]
    private Text labelText;
    [SerializeField]
    private Button button;

    private void Awake()
    {
        button.onClick.AddListener(ChangeText);
    }

    private void ChangeText()
    {
        labelText.text = "Button Clicked";
    }

}   