using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InteractionTest : Interactable
{
    public Sprite on;
    public Sprite off;

    private SpriteRenderer sr;
    private bool isOn;

	public override void Interact()
	{
		if (isOn)
		{
			sr.sprite = off;
		}
		else
		{
			sr.sprite = on;
		}
		isOn = !isOn;
	}

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		sr.sprite = off;
	}
}
