using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public GameObject refToMainUIText;
    public TextAsset dialogueText1;
    public TextAsset dialogueText2;
    public TextAsset dialogueText3;
    public string rawText;
    public string[] dialogue;
    private int currentSection;
    int option;

    public bool playing;

    /// <summary>
    /// options 1, 2, or 3
    /// </summary>
    /// <param name="option"></param>
    public void Play(int dialogueOption)
    {
        playing = true;
        option = dialogueOption;
        refToMainUIText.GetComponent<Button>().enabled = true;
    }

    public void NextSection()
    {
        currentSection++;
    }

    public void End()
    {
        refToMainUIText.GetComponent<Button>().enabled = false;
        currentSection = 0;
    }

    void Update()
    {
        if (playing == true)
        {
            //converts txt to array
            if (option == 1) rawText = dialogueText1.text;
            if (option == 2) rawText = dialogueText2.text;
            if (option == 3) rawText = dialogueText3.text;
            dialogue = rawText.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);

            refToMainUIText.GetComponent<Text>().text = dialogue[currentSection];
        }
    }
}
