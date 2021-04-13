using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance { get; set; }

    public GameObject dialoguePanel;
    [HideInInspector] public string npcName;
    [HideInInspector] public DialoguesRoot dialogues;
    [HideInInspector] public Line[] dialogueLines;

    Button continueButton;
    TMPro.TextMeshProUGUI dialogueText, nameText;
    int dialogueIndex;

	void Awake()
    {
        if (Instance != null && Instance != this)
		{
            Debug.LogWarning("Multiple instances of DialogueManager found! Problems?");
            Destroy(gameObject);
		}
        else
		{
            Instance = this;
		}

        continueButton = dialoguePanel.transform.Find("Continue").GetComponent<Button>();
        dialogueText = dialoguePanel.transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>();
        nameText = dialoguePanel.transform.Find("Name").GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        continueButton.onClick.AddListener(delegate { ContinueDialog(); });

        dialoguePanel.SetActive(false);
    }

    public void AddNewDialogue(int npcId, int dialogueId)
	{
        // Get the DialoguesRoot object from the json file {npcId}.json
        dialogues = JsonUtility.FromJson<DialoguesRoot>(Resources.Load<TextAsset>($"Dialogues/{npcId}").text);
        dialogueIndex = 0;
        // Get the dialogue with the dialogueId
        var dialogue = dialogues.dialogues.Where(x => x.id == dialogueId).FirstOrDefault();
        dialogueLines = dialogue.lines;
        //dialogueLines.AddRange(lines);
        //this.npcName = dialogueLines.name;

        CreateDialogue();
	}

    public void CreateDialogue()
	{
        dialogueText.text = dialogueLines[dialogueIndex].line;
        nameText.text = dialogueLines[dialogueIndex].name;
        // canviar el tipus amb dialogueLines[dialogueIndex].type (0 = normal, 1 = pensant, etc)
        dialoguePanel.SetActive(true);
	}

    public void ContinueDialog()
	{
		if (dialogueIndex < dialogueLines.Length - 1)
		{
            dialogueText.text = dialogueLines[++dialogueIndex].line;
            nameText.text = dialogueLines[dialogueIndex].name;
            // canviar el tipus amb dialogueLines[dialogueIndex].type (0 = normal, 1 = pensant, etc)
        }
        else
		{
            dialoguePanel.SetActive(false);
		}
	}
}
