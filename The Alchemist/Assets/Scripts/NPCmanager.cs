using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCmanager : MonoBehaviour {

    public string[] dialogue;
    public bool randomizeDialogue;

    private int dialogueCounter = 0; // if not going to randomize


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
