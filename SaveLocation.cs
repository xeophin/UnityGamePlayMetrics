using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;
using System.Xml;

/// <remarks>
/// This class acts as a tracker, writing an XML log of the player's
/// position. It could probably be used for other things as well.
/// 
/// Tracks a player avatar that is tagged as "Player".
/// </remarks>
public class SaveLocation : MonoBehaviour {

	/// <summary>
	/// The GameObject with the level data, used for some basic meta data.
	/// </summary>
	public GameObject levelData;
	
	/// <summary>
	/// In what interval the position of the player should be tracked,
	/// in seconds.
	/// </summary>
	public static float interval;

	private GameObject trackedPlayer;
	private XmlWriterSettings fragmentSettings;
	private string directory;
	private string logFilePath;
	private FileStream logfile;


	#region Startup Functions

	/// <summary>
	/// Needs to check whether a folder exists – and if it does not, create
	/// one. Probably unnecessary, but we'll do it for safety.
	/// </summary>
	void Awake () {
		directory = PlayerSettings.companyName + "/" + PlayerSettings.productName + "/tracker";
		if (!Directory.Exists (directory)) 
		{
			Directory.CreateDirectory (directory);
		}
	}
	
	/// <summary>
	/// Writes basic meta data at the beginning of the game, and prepares the
	/// wrapper file.
	/// </summary>
	void Start () {
		XmlWriterSettings wrapperSettings = new XmlWriterSettings ();
		wrapperSettings.Indent = true;
		string basename = DateTime.Now.ToOADate ().ToString ();
		string wrappername = directory + "/wrapper-" + basename + ".xml";
		logFilePath = directory + "/log-" + basename + ".xml";
		
		trackedPlayer = GameObject.FindGameObjectWithTag ("Player");
		
		string version = levelData.GetComponent<LevelData> ().levelVersion;
		string doctype = "<!DOCTYPE trackerdata \n [ \n <!ENTITY locations SYSTEM \"log-" + basename + ".xml\">\n ]>";
		
		Transform calibration = GameObject.FindGameObjectWithTag ("Calibration").transform;
		
		using (XmlWriter writer = XmlWriter.Create (wrappername, wrapperSettings)) {
			writer.WriteStartDocument ();
			writer.WriteRaw (doctype);
			writer.WriteStartElement ("trackerdata");
			
			// meta
			writer.WriteStartElement ("meta");
			
			
			writer.WriteStartElement ("starttime");
			writer.WriteValue (DateTime.Now);
			writer.WriteEndElement ();
			
			writer.WriteElementString ("levelversion", version);
			
			writer.WriteStartElement ("calibration");
			writer.WriteElementString ("x", calibration.position.x.ToString ());
			writer.WriteElementString ("y", calibration.position.y.ToString ());
			writer.WriteElementString ("z", calibration.position.z.ToString ());
			writer.WriteEndElement ();
			
			writer.WriteEndElement ();
			// /meta
			
			writer.WriteStartElement ("tracking");
			writer.WriteEntityRef ("locations");
			writer.Close ();
		}
		
		logfile = new FileStream (logFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
		
		
		fragmentSettings = new XmlWriterSettings ();
		fragmentSettings.ConformanceLevel = ConformanceLevel.Fragment;
		fragmentSettings.Indent = true;
		fragmentSettings.OmitXmlDeclaration = false;
		
		InvokeRepeating ("CollectData", 0, interval);
		
		
	}

	#endregion

	#region Main Function

	/// <summary>
	/// Logs the current position of the player.
	/// </summary>
	public void CollectData () {
		CollectData ("auto-position");
	}
	/// <summary>
	/// Logs the current position of the player at a specific event.
	/// </summary>
	/// <param name="eventName">
	/// The name of the event as a <see cref="System.String"/>.
	/// </param>
	public void CollectData (string eventName) {
		
		using (XmlWriter writer = XmlWriter.Create (logfile, fragmentSettings)) {
			writer.WriteStartElement ("location");
			
			writer.WriteStartAttribute ("runningTime");
			writer.WriteValue (Time.timeSinceLevelLoad);
			writer.WriteEndAttribute ();
			
			writer.WriteElementString ("eventname", eventName);
			
			writer.WriteStartElement ("x");
			writer.WriteValue (trackedPlayer.transform.position.x);
			writer.WriteEndElement ();
			
			writer.WriteStartElement ("y");
			writer.WriteValue (trackedPlayer.transform.position.y);
			writer.WriteEndElement ();
			
			writer.WriteStartElement ("z");
			writer.WriteValue (trackedPlayer.transform.position.z);
			writer.WriteEndElement ();
			
			writer.WriteEndElement ();
			writer.Flush ();
			
		}
		
		logfile.Flush ();
	}

	#endregion

	#region Event Handlers

	/// <summary>
	/// Logs the position of the player when he changes the direction.
	/// Can be called using BroadcastMessage(). (Serviervorschlag, effektive
	/// Implementation kann abweichen).
	/// </summary>
	void OnChangeDirection () {
		CollectData ("change-direction");
	}
	
	/// <summary>
	/// Logs the position of the player when he jumps.
	/// Can be called using BroadcastMessage().
	/// </summary>
	void OnJumpUp () {
		CollectData ("jump");
	}

	/// <summary>
	/// Logs the position of the player at the end of the game and
	/// closes the writer.
	/// </summary>
	public void OnApplicationQuit () {
		CollectData ("application-quit");
		//writer.Close ();
	}
	#endregion
}
