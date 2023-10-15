using Godot;
using Godot.Collections;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public partial class Game : Node
{
	private string savePath = "user://savegame.bin",
				   saveContents;

	Dictionary saveData = new Dictionary();

	public int globalHitPoints{ get; set;}
	
	public void SaveGame()
	{
		saveData.Add("HitPoints", globalHitPoints);

		saveContents = Json.Stringify(saveData);

		Godot.FileAccess.Open(savePath, Godot.FileAccess.ModeFlags.Write).StoreString(saveContents);
		
	}

	public void LoadGame()
	{
		if (Godot.FileAccess.FileExists(savePath))
		{
			Godot

		}

	}
	
}
