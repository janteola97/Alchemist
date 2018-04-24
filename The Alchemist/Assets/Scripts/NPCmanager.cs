using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour {

    public string[] dialogue;
    public bool randomizeDialogue;
    public Image NPCHead;
    public Image NPCRdyToTalk;

    private int dialogueCounter = 0; // if not going to randomize

    private void Start()
    {
        NPCHead.enabled = false;
        NPCRdyToTalk.enabled = false;
    }
    public string characterSpeak()
    {
        string tempDialogue = "code didn't work sorry";
        switch (randomizeDialogue)
        {
            case true:
                tempDialogue = dialogue[Random.Range(0, dialogue.Length)];
                break;

            case false:
                tempDialogue = dialogue[dialogueCounter];
                dialogueCounter++;
                if(dialogueCounter >= dialogue.Length)
                {
                    dialogueCounter = 0; 
                }
                break;
        }
        return tempDialogue;
            
    }





}
