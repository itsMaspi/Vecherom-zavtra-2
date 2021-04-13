using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialoguesRoot
{
	public Dialogue[] dialogues;
}

[System.Serializable]
public class Dialogue
{
	public int id;
	public Line[] lines;
}

[System.Serializable]
public class Line
{
	public int type;
	public string name;
	public string line;
}