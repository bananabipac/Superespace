//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;

/// <summary>
/// This is an example of a SwarmItemManager that handles creation of SwarmItem asteroids. 
/// It is also called to break up asteroids when they are hit by bullets.
/// </summary>
public class AsteroidManager : SwarmItemManager
{
	/// <summary>
	/// internal value for a faster calculation to determine if an asteroid is outside the field bounds 
	/// </summary>
	private float _fieldRadiusSquared;
	
	/// <summary>
	/// Initial count for the small asteroids 
	/// </summary>
	public int initialSmallAsteroidCount;
	
	/// <summary>
	/// Initial count for the medium asteroids 
	/// </summary>
	public int initialMediumAsteroidCount;
	
	/// <summary>
	/// Initial count for the large asteroids  
	/// </summary>	
	public int initialLargeAsteroidCount;
	
	/// <summary>
	/// The minimum spin to use for the random spin calculation 
	/// </summary>
	public Vector3 minimumSpin;
	
	/// <summary>
	/// The maximum spin to use for the random spin calculation 
	/// </summary>
	public Vector3 maximumSpin;
	
	/// <summary>
	/// The minimum velocity to use for the random velocity calculation (2D: x and y) 
	/// </summary>
	public Vector2 minimumVelocity;

	/// <summary>
	/// The maximum velocity to use for the random velocity calculation (2D: x and y) 
	/// </summary>
	public Vector2 maximumVelocity;
	
	/// <summary>
	/// the size of the asteroid field 
	/// </summary>
	public float fieldRadius;
	
	/// <summary>
	/// accessor for the square of the field radius 
	/// </summary>
	public float FieldRadiusSquared { get { return _fieldRadiusSquared; } }
	
	/// <summary>
	/// Overrides the SwarmItemManager's Initialize method 
	/// </summary>
	public override void Initialize()
	{
		// call the SwarmItemManager Initialize method
		base.Initialize();
		
		// cache the squared value of the field radius for faster processing
		_fieldRadiusSquared = fieldRadius * fieldRadius;
		
		// initialize the asteroid types
		InitializeAsteroidGroup(Asteroid.ASTEROID_TYPE.Small, initialSmallAsteroidCount);		
		InitializeAsteroidGroup(Asteroid.ASTEROID_TYPE.Medium, initialMediumAsteroidCount);		
		InitializeAsteroidGroup(Asteroid.ASTEROID_TYPE.Large, initialLargeAsteroidCount);	
	}
	
	/// <summary>
	/// Sets up the asteroids of a given size 
	/// </summary>
	/// <param name="asteroidType">The prefab type to set up</param>
	/// <param name="count">The number of asteroids to set up for this type</param>
	private void InitializeAsteroidGroup(Asteroid.ASTEROID_TYPE asteroidType, int count)
	{
		for (int i=0; i<count; i++)
		{
			// activate a SwarmItem from the manager
			Asteroid asteroid = (Asteroid)ActivateItem((int)asteroidType);
	
			if (asteroid != null)
			{
				// set asteroid with random spin, velocity, and position
				asteroid.Set(this, asteroidType, GetRandomSpin(), GetRandomVelocity(), GetRandomPosition());
			}
		}		
	}
	
	/// <summary>
	/// Returns a random vector between the minimum and maximum spin 
	/// </summary>
	/// <returns>Returns random spin</returns>
	private Vector3 GetRandomSpin()
	{
		return new Vector3(
		                   UnityEngine.Random.Range(minimumSpin.x, maximumSpin.x),
		                   UnityEngine.Random.Range(minimumSpin.y, maximumSpin.y),
		                   UnityEngine.Random.Range(minimumSpin.z, maximumSpin.z)
		                   );
	}

	/// <summary>
	/// Returns a random 3D vector between the minimum and maximum velocity in 2D 
	/// </summary>
	/// <returns>Returns random velocity</returns>
	private Vector3 GetRandomVelocity()
	{
		// get a random velocity between minimum and maximum velocity
		return new Vector3(
		                   UnityEngine.Random.Range(minimumVelocity.x, maximumVelocity.x),
		                   UnityEngine.Random.Range(minimumVelocity.y, maximumVelocity.y),
		                   0
		                   );		
	}
	
	/// <summary>
	/// Gets a random position within the field radius 
	/// </summary>
	/// <returns>Returns random position</returns>
	private Vector3 GetRandomPosition()
	{
		Vector2 position = UnityEngine.Random.insideUnitCircle * fieldRadius;
		return new Vector3(position.x, position.y, 0);
	}	

	/// <summary>
	/// Creates new asteroids of a smaller size where the old asteroid was,
	/// giving the illusion of a crumbling rock
	/// </summary>
	/// <param name="asteroidType">The type of prefab for the old asteroid</param>
	/// <param name="position">The position of the old asteroid</param>
	public void BreakUpAsteroid(Asteroid.ASTEROID_TYPE asteroidType, Vector3 position)
	{
		Asteroid asteroid;
		
		switch (asteroidType)
		{
		case Asteroid.ASTEROID_TYPE.Large:
			// break large asteroid into two medium ones at the old asteroid's position
			
			asteroid = (Asteroid)ActivateItem((int)Asteroid.ASTEROID_TYPE.Medium);
			if (asteroid != null)
				asteroid.Set(this, Asteroid.ASTEROID_TYPE.Medium, GetRandomSpin(), GetRandomVelocity(), position);
			
			asteroid = (Asteroid)ActivateItem((int)Asteroid.ASTEROID_TYPE.Medium);
			if (asteroid != null)
				asteroid.Set(this, Asteroid.ASTEROID_TYPE.Medium, GetRandomSpin(), GetRandomVelocity(), position);
			break;
			
		case Asteroid.ASTEROID_TYPE.Medium:
			// break medium asteroid into two small ones at the old asteroid's position
			
			asteroid = (Asteroid)ActivateItem((int)Asteroid.ASTEROID_TYPE.Small);
			if (asteroid != null)
				asteroid.Set(this, Asteroid.ASTEROID_TYPE.Small, GetRandomSpin(), GetRandomVelocity(), position);

			asteroid = (Asteroid)ActivateItem((int)Asteroid.ASTEROID_TYPE.Small);
			if (asteroid != null)
				asteroid.Set(this, Asteroid.ASTEROID_TYPE.Small, GetRandomSpin(), GetRandomVelocity(), position);
			break;
			
		case Asteroid.ASTEROID_TYPE.Small:
			// do nothing here since small asteroids are the last and smallest size
			break;
		}
	}
}
