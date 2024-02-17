using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    public GameObject panel;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            panel.SetActive(!panel.activeSelf);
        }
    }
}
