//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;

/// <summary>
/// SwarmItem that handles the bullets from the ship. Processes collisions and creates explosions. 
/// </summary>
public class Bullet : SwarmItem
{
	/// <summary>
	/// velocity of the bullet
	/// </summary>
	private Vector3 _velocity;
	
	/// <summary>
	/// original scale of the bullet to preserve scale when parenting to the active or inactive lists
	/// </summary>
	private Vector3 _originalScale;

	/// <summary>
	/// Overrides the SwarmItem's Initialize method and sets the originalScale 
	/// </summary>
	public override void Initialize(SwarmItemManager swarmItemManager, int prefabIndex, bool debugEvents)
	{
		base.Initialize(swarmItemManager, prefabIndex, debugEvents);
		
		_originalScale = _thisTransform.localScale;
	}
	
	/// <summary>
	/// Sets the bullet's velocity 
	/// </summary>
	/// <param name="velocity">Velocity of bullet</param>
	public void Fire(Vector3 velocity)
	{
		_velocity = velocity;
	}
	
	/// <summary>
	/// Called after being parented to the active or inactive transforms 
	/// </summary>
	public override void OnSetParentTransform()
	{
		// reset the scale back to its original size
		_thisTransform.localScale = _originalScale;
	}
	
	/// <summary>
	/// Called every frame to move the bullet and check if it is in bounds 
	/// </summary>
	public override void FrameUpdate()
	{
		// move bullet
		Position += (_velocity * Time.deltaTime);
		
		// check in bounds
		if (Position.sqrMagnitude >= Main.Instance.asteroidManager.FieldRadiusSquared)
		{
			// kill bullet if out of bounds
			Kill();
		}
		
		// process base FrameUpdate in case the bullet has a life span
		base.FrameUpdate();
	}
	
	/// <summary>
	/// Checks for collisions with asteroids 
	/// </summary>
	/// <param name="otherCollider">Asteroid's collider</param>
	void OnTriggerEnter(Collider otherCollider)
	{
		// in this simple example, we know we will only collide with asteroids, so we get the asteroid from the collider
		Asteroid asteroid = (Asteroid)otherCollider.gameObject.GetComponent(typeof(Asteroid));
		
		// kill the asteroid
		if (asteroid != null)
			asteroid.Kill();
		
		// kill the bullet
		Kill();
	}
}
