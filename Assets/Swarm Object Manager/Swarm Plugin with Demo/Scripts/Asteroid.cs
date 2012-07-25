//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;

/// <summary>
/// Asteroid class that inherits from the SwarmItem. Its manager is the AsteroidManager. 
/// </summary>
public class Asteroid : SwarmItem
{
	/// <summary>
	/// the SwarmItemManager for this SwarmItem
	/// </summary>
	private AsteroidManager _asteroidManager;
	
	/// <summary>
	/// the type of asteroid (size)
	/// </summary>
	private ASTEROID_TYPE _asteroidType; 
	
	/// <summary>
	/// the rotation of the asteroid
	/// </summary>
	private Vector3 _rotation;
	
	/// <summary>
	/// the velocity of the asteroid
	/// </summary>
	private Vector3 _velocity;
	
	/// <summary>
	/// the scale of the asteroid after it is parented in the manager's active list
	/// </summary>
	private Vector3 _originalScale;
	
	/// <summary>
	/// These types should correspond with the prefabs set in the editor for the asteroid manager 
	/// </summary>
	public enum ASTEROID_TYPE
	{
		Small = 0,
		Medium = 1,
		Large = 2
	}	
	
	/// <summary>
	/// Overrides the SwarmItem Initialize method, setting the originalScale member 
	/// </summary>
	public override void Initialize(SwarmItemManager swarmItemManager, int prefabIndex, bool DebugEvents)
	{
		// initialize the SwarmItem base first
		base.Initialize(swarmItemManager, prefabIndex, DebugEvents);
		
		// set the scale to be used after parenting to the active list
		_originalScale = _thisTransform.localScale;
	}
	
	/// <summary>
	/// Sets the asteroids parameters 
	/// </summary>
	/// <param name="asteroidManager">The manager for this item</param>
	/// <param name="asteroidType">The type of asteroid (size)</param>
	/// <param name="rotation">Rotation of the asteroid</param>
	/// <param name="velocity">Velocity of the asteroid</param>
	/// <param name="position">Position of the asteroid</param>
	public void Set(AsteroidManager asteroidManager, ASTEROID_TYPE asteroidType, Vector3 rotation, Vector3 velocity, Vector3 position)
	{
		_asteroidManager = asteroidManager;
		_asteroidType = asteroidType;
		_rotation = rotation;
		_velocity = velocity;
		
		// We use the Accessor for this member to update the transform position
		Position = position;		
	}
	
	/// <summary>
	/// Overrides the SwarmItem FrameUpdate to move and rotate the asteroid 
	/// </summary>
	public override void FrameUpdate()
	{
		// move asteroid
		Position += (_velocity * Time.deltaTime);
		
		// reflect the asteroid's velocity within the field so that asteroid's aren't flying off into infinity
		if (Position.sqrMagnitude >= _asteroidManager.FieldRadiusSquared)
		{
			// save the normalized position since we'll be using it twice below. 
			// this cuts down on the normalization calculations which can be expensive,
			// especially when done every frame.
			Vector3 normalizedPosition = Position.normalized;
			
			// clip the asteroid's position to the field radius
			Position = normalizedPosition * _asteroidManager.fieldRadius;
			
			// reflect the velocity across the inverted normal (makes the asteroid bounce back)
			_velocity = Vector3.Reflect(_velocity, -normalizedPosition);			
		}

		// rotate asteroid
		_thisTransform.Rotate(_rotation * Time.deltaTime);
		
		// call the base FrameUpdate in case this asteroid has a life span
		base.FrameUpdate();
	}
	
	/// <summary>
	/// Called after being activated or deactivated 
	/// </summary>
	protected override void OnStateChange ()
	{
		switch (_state)
		{
		case STATE.Inactive:
			// the asteroid was destroyed, so create smaller ones in its place
			_asteroidManager.BreakUpAsteroid(_asteroidType, Position);
			
			// create explosion
			Explosion explosion = (Explosion)Main.Instance.explosionManager.ActivateItem();
			if (explosion != null)
				explosion.Position = Position;
			break;
		}
	}
	
	/// <summary>
	/// Called after being parented to the active or inactive lists
	/// </summary>
	public override void OnSetParentTransform()
	{
		// set the scale back to its original size
		_thisTransform.localScale = _originalScale;
	}
}
