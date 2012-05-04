//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;

/// <summary>
/// The Ship class displays a ship and takes input to fire bullets
/// </summary>
public class Ship : MonoBehaviour
{
	/// <summary>
	/// the cached transform of the ship
	/// </summary>
	private Transform _thisTransform;
	
	/// <summary>
	/// the speed to turn the ship
	/// </summary>
	public float turnSpeed;
	
	/// <summary>
	/// the speed of the bullets
	/// </summary>
	public float bulletSpeed;
	
	/// <summary>
	/// Sets up the ship
	/// </summary>
	public void Initialize()
	{
		// cache the transform for faster access
		_thisTransform = this.transform;
	}
	
	/// <summary>
	/// Called every frame from the Main class 
	/// </summary>
	public void FrameUpdate()
	{
		// check for turning left
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			_thisTransform.RotateAround(Vector3.back, -turnSpeed * Time.deltaTime);
		}
		
		// check for turning right
		if (Input.GetKey(KeyCode.RightArrow))
		{
			_thisTransform.RotateAround(Vector3.back, turnSpeed * Time.deltaTime);
		}
		
		// fire a bullet
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
		{
			Bullet bullet = (Bullet)Main.Instance.bulletManager.ActivateItem();
			if (bullet != null)
				bullet.Fire(_thisTransform.up * bulletSpeed);
		}
	}
	
}
