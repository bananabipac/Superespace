//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;

/// <summary>
/// The Main class initializes and updates all managers. It uses singleton code to allow any object to reference any other object
/// </summary>
public class Main : MonoBehaviour
{
	#region Singleton Code
	/// <summary>
	/// internal instance variable set in the Awake method 
	/// </summary>
	private static Main _instance = null;
	
	/// <summary>
	/// The instance of the singleton. Should be only one in a scene. 
	/// </summary>
	public static Main Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(Main)) as Main;

				if (_instance == null && Application.isEditor)
					Debug.LogError("Could not locate a Main object. You have to have exactly one Main in the scene.");
			}

			return _instance;
		}
	}	
	
	/// <summary>
	/// Sets the instance 
	/// </summary>
	void Awake()
	{
		// See if we are a superfluous instance:
		if (_instance != null)
		{
			Debug.LogError("You can only have one instance of the Main singleton object in existence.");
		}
		else
			_instance = this;		
	}	
	#endregion		
	
	/// <summary>
	/// ship control 
	/// </summary>
	public Ship ship;
	
	/// <summary>
	/// reference to the asteroid manager
	/// </summary>
	public AsteroidManager asteroidManager;
	
	/// <summary>
	/// bullet manager (we don't need an inherited manager for this since the base class handles everything for us)
	/// </summary>
	public SwarmItemManager bulletManager;
	
	/// <summary>
	/// explosion manager (we don't need an inherited manager for this since the base class handles everything for us)
	/// </summary>
	public SwarmItemManager explosionManager;
	
	/// <summary>
	/// Initializes the ship and all swarm managers 
	/// </summary>
	void Start()
	{
		ship.Initialize();
		asteroidManager.Initialize();
		bulletManager.Initialize();
		explosionManager.Initialize();
	}
	
	/// <summary>
	/// Updates all managers. Note that this is more efficient that having an Update in every
	/// manager class because it requires less reflection usage. It also allows us to explicitly
	/// state the order of FrameUpdates.
	/// </summary>
	void Update()
	{
		ship.FrameUpdate();
		asteroidManager.FrameUpdate();
		bulletManager.FrameUpdate();
		explosionManager.FrameUpdate();
	}
}
