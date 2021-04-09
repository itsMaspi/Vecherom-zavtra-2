using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance { get; set; }

    public GameObject dialoguePanel;
    [HideInInspector] public string npcName;
    [HideInInspector] public List<string> dialogueLines = new List<string>();

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

    public void AddNewDialogue(string[] lines, string npcName)
	{
        dialogueIndex = 0;
        dialogueLines = new List<string>();
        dialogueLines.AddRange(lines);
        this.npcName = npcName;

        Debug.Log(dialogueLines.Count);
        CreateDialogue();
	}

    public void CreateDialogue()
	{
        dialogueText.text = dialogueLines[dialogueIndex];
        nameText.text = npcName;
        dialoguePanel.SetActive(true);
	}

    public void ContinueDialog()
	{
		if (dialogueIndex < dialogueLines.Count-1)
		{
            dialogueText.text = dialogueLines[++dialogueIndex];
		}
        else
		{
            dialoguePanel.SetActive(false);
		}
	}
}
