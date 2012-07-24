//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;

/// <summary>
/// Class to handle particle system SwarmItems
/// </summary>
public class Explosion : SwarmItem
{
	/// <summary>
	/// particle emitter cached
	/// </summary>
	private ParticleEmitter emitter;
	
	/// <summary>
	/// Overrides SwarmItem's Initialize Method and caches the emitter 
	/// </summary>
	public override void Initialize(SwarmItemManager swarmItemManager, int prefabIndex, bool debugEvents)
	{
		base.Initialize(swarmItemManager, prefabIndex, debugEvents);

		// cache the emitter for later use
		emitter = (ParticleEmitter)gameObject.GetComponent(typeof(ParticleEmitter));
		emitter.emit = false;
	}
	
	/// <summary>
	/// Called after the explosion is activated or deactivated 
	/// </summary>
	protected override void OnStateChange()
	{
		switch (_state)
		{
		case STATE.Active:
			// turn on the emitter if active
			emitter.emit = true;
			break;
			
		case STATE.Inactive:
			// clear the particles and turn off emitter if inactive
			emitter.ClearParticles();
			emitter.emit = false;
			break;
		}
	}
}
