using UnityEngine;
using System;

/// <summary>
/// This is probably not a very elegant solution. Shouldn't that be done with
/// a struct?
/// </summary>
public class GameData : MonoBehaviour {

	static string language;
	static string gameName;
	static string version;
	static string company;


	/// <summary>
	/// Keeps the object in game, therefore retaining all necessary data.
	/// </summary>
	void Awake () {
		DontDestroyOnLoad (this);
	}
}
