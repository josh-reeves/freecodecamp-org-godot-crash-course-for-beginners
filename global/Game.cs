using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public partial class Game : Node
{
	private static string filePath = "user://savegame.bin",
				   		  saveContents;

	private static Godot.FileAccess saveFile;
	private static Godot.Collections.Dictionary saveData = new Dictionary();
	private static Json jsonHandle = new Json();

	public static int globalHitPoints{ get; set;} = 3;
	
	public static void SaveGame()
	{
		saveData.Clear();
		saveData.Add("HitPoints", globalHitPoints);

		saveContents = Json.Stringify(saveData);

		saveFile = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write);
		saveFile.StoreString(saveContents);
		
	}

	public static void LoadGame()
	{
		if (Godot.FileAccess.FileExists(filePath))
		{
			saveFile = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);

			while (saveFile.GetPosition() < saveFile.GetLength())
			{
				if (jsonHandle.Parse(saveFile.GetLine()) == Error.Ok)
				{
					saveData = (Godot.Collections.Dictionary)jsonHandle.Data;

				}

			}

			foreach (var (key, value) in saveData )
			{
				if ((string)key == "HitPoints")
				{
					globalHitPoints = (int)value;

				}

			}

		}

	}
	
}

public abstract partial class Stage : Node2D
{
	// Variable Properties:
	public int collectableTotal {get; set;}

	// Object Properties:
	public Player player {get; set;}

}
