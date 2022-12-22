using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject refToMainPanel;
    public GameObject refToControlsPanel;

    void Start()
    {
        refToMainPanel.SetActive(true);
        refToControlsPanel.SetActive(false);
    }

    public void PlayButton()
    {

    }

    public void ControlsButton()
    {
        refToMainPanel.SetActive(false);
        refToControlsPanel.SetActive(true);
    }

    public void BackButton()
    {
        refToMainPanel.SetActive(true);
        refToControlsPanel.SetActive(false);
    }
}
