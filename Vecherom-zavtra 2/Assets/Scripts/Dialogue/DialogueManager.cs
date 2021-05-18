using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
	enum DialogueTypes
	{
        Normal,
        Thinking,
        Shouting
	}
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

	void OnApplicationQuit()
	{
        Utils.DeleteUserInfo();
	}

	public void AddNewDialogue(int npcId, int dialogueId)
	{
        // Get the DialoguesRoot object from the json file {npcId}.json
        dialogues = JsonUtility.FromJson<DialoguesRoot>(Resources.Load<TextAsset>($"Dialogues/{npcId}").text);
        dialogueIndex = 0;
        // Get the dialogue with the dialogueId
        var dialogue = dialogues.dialogues.Where(x => x.id == dialogueId).FirstOrDefault();
        dialogueLines = dialogue.lines;

        string nick = Utils.GetUserNickname();
        foreach (var line in dialogueLines)
		{
            // Replace player name in dialogue name
            if (line.name.Equals("<player>")) line.name = nick;
            // Replace player name in the line
            line.line = line.line.Replace("<player>", nick);
            // Manage dialogue type
            ManageDialogueType(line.type);
		}

        CreateDialogue();
	}

    public void CreateDialogue()
	{
        dialogueText.text = dialogueLines[dialogueIndex].line;
        nameText.text = dialogueLines[dialogueIndex].name;
        ManageDialogueType(dialogueLines[dialogueIndex].type);
        dialoguePanel.SetActive(true);
	}

    public void ContinueDialog()
	{
		if (dialogueIndex < dialogueLines.Length - 1)
		{
            dialogueText.text = dialogueLines[++dialogueIndex].line;
            nameText.text = dialogueLines[dialogueIndex].name;
            ManageDialogueType(dialogueLines[dialogueIndex].type);
        }
        else
		{
            dialoguePanel.SetActive(false);
		}
	}

    private void ManageDialogueType(int type)
	{
		switch ((DialogueTypes)type)
		{
            case DialogueTypes.Normal:
                dialoguePanel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.3f);
                break;
            case DialogueTypes.Thinking:
                dialoguePanel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);
                break;
            case DialogueTypes.Shouting:
                dialoguePanel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
                break;
            default:
				break;
		}
	}
}
