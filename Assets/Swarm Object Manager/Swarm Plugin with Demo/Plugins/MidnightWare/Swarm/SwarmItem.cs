//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;

/// <summary>
/// This class serves as the base for all swarm objects. SwarmItems are managed by a SwarmItemManager.
/// A SwarmItem should be activated by its manager and killed through this class.
/// </summary>
public class SwarmItem : MonoBehaviour
{
	/// <summary>
	/// the state of the item (inactive or active)
	/// </summary>
	protected STATE _state;				
	
	/// <summary>
	/// the item's parent manager
	/// </summary>
	protected SwarmItemManager _swarmItemManager;
	
	/// <summary>
	/// cached transform for faster lookups
	/// </summary>
	protected Transform _thisTransform;	
	
	/// <summary>
	/// index of the manager's prefab for this item
	/// </summary>
	protected int _prefabIndex;
	
	/// <summary>
	/// the amount of time left for the item to live
	/// </summary>
	protected float _lifeSpanLeft;
	
	/// <summary>
	/// debug events that occur with this item (set in manager) 
	/// </summary>
	protected bool _debugEvents;
	
	/// <summary>
	/// An item can only be active or inactive 
	/// </summary>
	public enum STATE
	{
		/// <summary>
		/// Inactive means that the item is not visible and not processed.
		/// It will reside in the manager's inactive list.
		/// </summary>
		Inactive,
		
		/// <summary>
		/// Active means the item will be visible and processed in frameupdate.
		/// It will reside in the manager's active list.
		/// </summary>
		Active
	}
	
	/// <summary>
	/// sets the random length of time the item is to live (between minimum and maximum). 
	/// zero or lower means the item will live until killed manually.
	/// if both minimum and maximum are identical, then the lifespan is fixed and not random.
	/// </summary>
	public float minimumLifeSpan = 0;
	public float maximumLifeSpan = 0;
	
	/// <summary>
	/// Sets the state of the item to inactive or active 
	/// </summary>
	public STATE State
	{
		get
		{
			return _state;
		}
		set
		{
			// save whether the state was changed
			bool _stateChanged = (_state != value);
			
			_state = value;
			
			switch (_state)
			{
			case STATE.Inactive:
				// turn off the item
				gameObject.SetActiveRecursively(false);
				break;
				
			case STATE.Active:
				// turn on the item and reset its life span
				gameObject.SetActiveRecursively(true);
				_lifeSpanLeft = UnityEngine.Random.Range(minimumLifeSpan, maximumLifeSpan);
				break;
			}
			
			// if the state changed, call the OnStateChange method
			if (_stateChanged)
				OnStateChange();
		}
	}
	
	/// <summary>
	/// Method that is called if the state is changed. This can be used
	/// to further process an item when it is activated or deactivated.
	/// </summary>
	protected virtual void OnStateChange()
	{
	}
	
	/// <summary>
	/// Accessor to the transform of this item 
	/// </summary>
	public Transform ThisTransform
	{
		get
		{
			return _thisTransform;
		}
	}
	
	/// <summary>
	/// The absolute position of an item in world space 
	/// </summary>
	public Vector3 Position
	{
		get
		{
			return _thisTransform.position;
		}
		set
		{
			_thisTransform.position = value;
		}
	}	
	
	/// <summary>
	/// The index of the manager's prefab 
	/// </summary>
	public int PrefabIndex
	{
		get
		{
			return _prefabIndex;
		}
	}
	
	/// <summary>
	/// Sets up an item upon creation. 
	/// </summary>
	/// <param name="swarmItemManager">The item's manager</param>
	/// <param name="prefabIndex">The index of the manager's prefab</param>
	public virtual void Initialize(SwarmItemManager swarmItemManager, int prefabIndex, bool debugEvents)
	{
		_swarmItemManager = swarmItemManager;
		_prefabIndex = prefabIndex;
		_debugEvents = debugEvents;
		
		_thisTransform = this.transform;
		
		State = STATE.Inactive;
	}
	
	/// <summary>
	/// This method is called by the manager after the item is moved to either the active or inactive list.
	/// It is often useful to modify an item's position, rotation, or scale here since these values will
	/// be set to Vector3.zero, Quaternion.Identity, and Vector3.one, respectively.
	/// </summary>
	public virtual void OnSetParentTransform()
	{
	}
	
	/// <summary>
	/// This is the method that should be called when an item is no longer needed.
	/// Killing will recycle the item to the manager's inactive list, not send it to the garbage collector.
	/// </summary>
	public virtual void Kill()
	{
		State = STATE.Inactive;

		_swarmItemManager.DeactiveItem(this);
	}
	
	/// <summary>
	/// This is called before an object is destroyed by a prune event from the manager.
	/// You should call Destroy on any objects in your inherited class that need their memory freed
	/// using an overload of this method.
	/// </summary>
	public virtual void PreDestroy()
	{
	}
	
	/// <summary>
	/// This method is called from the manager every frame 
	/// </summary>
	public virtual void FrameUpdate()
	{
		// only process life left if the life span is positive
		if (_lifeSpanLeft > 0)
		{
			// decrement life, if none left, then kill the item
			_lifeSpanLeft -= Time.deltaTime;
			if (_lifeSpanLeft <= 0)
			{
				Kill();
				
				if (_debugEvents)
					Debug.Log("Lifespan expired for item " + name + " at frame: " + Time.frameCount);
			}
		}
	}
}