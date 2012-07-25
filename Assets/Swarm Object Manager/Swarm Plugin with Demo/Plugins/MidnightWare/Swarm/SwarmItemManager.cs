//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// The SwarmItemManager handles SwarmItem objects. It actively recycles SwarmItems so that no garbage
/// collection is necessary. It will dynamically add new objects as required, pulling from inactive lists
/// if any recycled SwarmItems are available. You can have multiple SwarmItemManagers in a scene to handle
/// different types of objects. Alternatively, you could handle all your object types within a single
/// SwarmItemManager.
/// 
/// The manager can limit each of its lists to an optional maximum item count. If this limit greater than zero,
/// then the manager won't instantiate any more objects in a list once it has reached the maximum count. If the
/// limit is zero, the manager will keep instantiating new objects as neccessary.
/// 
/// Each item list has its own optional pruning functionality. When an item is pruned it is sent to the garbage collector. 
/// If the number of the inactive items of a prefab exceed the set threshold, a pruning timer will kick in. 
/// If the number of the inactive items drop back below the threshold, the pruning timer will shut back off. 
/// When the pruning timer countdown expires, the inactive list will be pruned to the prune percentage set 
/// for the item. You can shut off pruning by setting the inactivePrunePercentage to zero.
/// </summary>
public class SwarmItemManager : MonoBehaviour
{
	/// <summary>
	/// This internal class represents the active and inactive lists of a SwarmItem type.
	/// Since SwarmItemManager's can handle multiple types of SwarmItems, there is
	/// an active and inactive list for each.
	/// </summary>
	protected class PrefabItemLists
	{
		/// <summary>
		/// linked list of active items. Faster than a List since we may need to remove items in the middle of the list
		/// </summary>
		public LinkedList<SwarmItem> activeItems;
		
		/// <summary>
		/// stack of inactive items. It doesn't matter which of the inactive items we pull, so we always pop the top for efficiency
		/// </summary>
		public Stack<SwarmItem> inactiveItems;
		
		/// <summary>
		/// the total number of items (active and inactive in this list) 
		/// </summary>
		public int itemCount;
		
		/// <summary>
		/// the amount of time in seconds left before the inactive list is pruned 
		/// (unless the number of inactive items drops below the threshold)
		/// </summary>
		public float inactivePruneTimeLeft;
		
		/// <summary>
		/// Initializes the lists 
		/// </summary>
		public PrefabItemLists ()
		{
			activeItems = new LinkedList<SwarmItem>();
			inactiveItems = new Stack<SwarmItem>();
		}
	}
	
	/// <summary>
	/// This class is used to wrap the prefab so that you can set the maximum
	/// count for each item and set the pruning parameters, both optional.
	/// </summary>
	[Serializable]
	public class PrefabItem
	{
		/// <summary>
		/// prefab game object of the item
		/// </summary>
		public GameObject prefab;
		
		/// <summary>
		/// maximum count for this item (0 = no limit, postive number = the list will be limited to the max item count) 
		/// </summary>
		public int maxItemCount = 0;
		
		/// <summary>
		/// the percentage of total items in the list that triggers the prune timer countdown.
		/// if the inactive item count exceeds this percentage, the timer is triggered.
		/// (ex: 0.7f = when the inactive item count is greater than 70% of all the items (active and inactive)
		/// the prune timer will kick in).
		/// defaults to turning on the pruning timer only when every item is inactive.
		/// </summary>
		public float inactiveThreshold = 1.0f;
	
		/// <summary>
		/// the amount of time in seconds to count down after the prune threshold is exceeded.
		/// if the inactive count drops below the threshold before this timer has expired, the
		/// timer will be shut off and reset.
		/// (ex: 5.0f = 5 seconds after the prune threshold is exceeded, the inactive list will be pruned).
		/// defaults to pruning immediately when threshold is reached.
		/// </summary>
		public float inactivePruneTimer = 0.0f;		

		/// <summary>
		/// the percentage to prune the inactive list when the prune timer countdown expires.
		/// the amount is the percentage of the inactive list, not the total item count.
		/// (ex: 0.3f = approximately one third of the inactive items will be destroyed when the prune timer expires).
		/// defaults to no pruning.
		/// </summary>
		public float inactivePrunePercentage = 0.0f;
	}
	
	/// <summary>
	/// internal member to set up a SwarmItem
	/// </summary>
	private SwarmItem _item;
	
	/// <summary>
	/// internal member to instantiate a SwarmItem
	/// </summary>
	private GameObject _go;
	
	/// <summary>
	/// internal member to cache a SwarmItem's transform
	/// </summary>
	private Transform _transform;
	
	/// <summary>
	/// internal member that stores the parent transform of all active items. This is used as a visual aid in the editor. 
	/// </summary>
	private Transform _activeParentTransform;
	
	/// <summary>
	/// internal member that stores the parent transform of all inactive items. This is used as a visual aid in the editor.
	/// </summary>
	private Transform _inactiveParentTransform;
	
	/// <summary>
	/// internal member that keeps track of the total items created 
	/// </summary>
	private int _itemCount;
	
	/// <summary>
	/// the array of prefab item lists. Each prefab gets a set of inactive and active lists
	/// </summary>
	protected PrefabItemLists [] _prefabItemLists;
	
	/// <summary>
	/// flag to show important events happening in this manager 
	/// </summary>
	public bool DebugEvents;
	
	/// <summary>
	/// the array of prefabs with their maximum item counts. This is set in the editor
	/// </summary>
	public PrefabItem [] itemPrefabs; 
	
	/// <summary>
	/// Sets up the lists for each SwarmItem type. Also creates the parent transform for the active and inactive objects 
	/// </summary>
	public virtual void Initialize()
	{
		// warn the user if no prefabs were set up. There would be no need for a manager without SwarmItems
		if (itemPrefabs.Length == 0)
		{
			Debug.Log("WARNING! No Item Prefabs exists for " + gameObject.name + " -- Errors will occur.");
		}
		
		// make sure all the thresholds and percentages are clamped between 0 and 1.0f
		foreach (PrefabItem itemPrefab in itemPrefabs)
		{
			itemPrefab.inactiveThreshold = Mathf.Clamp01(itemPrefab.inactiveThreshold);
			itemPrefab.inactivePrunePercentage = Mathf.Clamp01(itemPrefab.inactivePrunePercentage);
		}
		
		// initialize the prefab item lists
		_prefabItemLists = new PrefabItemLists[itemPrefabs.Length];
		for (int i=0; i<_prefabItemLists.Length; i++)
		{
			_prefabItemLists[i] = new PrefabItemLists();
			_prefabItemLists[i].itemCount = 0;
			_prefabItemLists[i].inactivePruneTimeLeft = 0;
		}
		
		// create the active objects parent transform
		_go = new GameObject("Active Items");
		_activeParentTransform = _go.transform;
		_activeParentTransform.parent = this.transform;
		_activeParentTransform.localPosition = Vector3.zero;
		_activeParentTransform.localRotation = Quaternion.identity;
		_activeParentTransform.localScale = Vector3.one;

		// create the inactive objects parent transform;
		_go = new GameObject("Inactive Items");
		_inactiveParentTransform = _go.transform;
		_inactiveParentTransform.parent = this.transform;
		_inactiveParentTransform.localPosition = Vector3.zero;
		_inactiveParentTransform.localRotation = Quaternion.identity;
		_inactiveParentTransform.localScale = Vector3.one;
	}
	
	/// <summary>
	/// Overloaded form of ActivateItem that assumes you just want the first SwarmItem type (first prefab) 
	/// </summary>
	/// <returns>Returns the newly created SwarmItem</returns>
	public virtual SwarmItem ActivateItem()
	{
		return ActivateItem(0);
	}
	
	/// <summary>
	/// Activates a SwarmItem base on the prefab index (type). If there is an inactive item,
	/// the manager will recycle that first, otherwise it will instantiate a new item
	/// </summary>
	/// <param name="itemPrefabIndex">The index of the prefab to use (type of SwarmItem)</param>
	/// <returns>Returns the newly created SwarmItem</returns>
	public virtual SwarmItem ActivateItem(int itemPrefabIndex)
	{
		// we have exceeded the maximum item count for this prefab (if a limit is set)
		// so return nothing
		if (_prefabItemLists[itemPrefabIndex].activeItems.Count == itemPrefabs[itemPrefabIndex].maxItemCount && itemPrefabs[itemPrefabIndex].maxItemCount > 0)
		{
			if (DebugEvents)
				Debug.Log("Could not activate item because the count [" + _prefabItemLists[itemPrefabIndex].activeItems.Count + "] is at the maximum number for this item type at frame: " + Time.frameCount);

			return null;
		}
		
		if (_prefabItemLists[itemPrefabIndex].inactiveItems.Count > 0)
		{
			// there is an inactive item so we recycle it
			
			// pop off the inactive stack
			_item = _prefabItemLists[itemPrefabIndex].inactiveItems.Pop();
			
			// queue to the end of the active list
			_prefabItemLists[itemPrefabIndex].activeItems.AddLast(_item);
			
			if (DebugEvents)
				Debug.Log("Recycled item " + _item.name + " at frame: " + Time.frameCount);
		}
		else
		{
			// no inactive item availble, so create a new one
			
			// instantiate item
			_item = InstantiateItem(itemPrefabIndex);
			
			// queue to the end of the active list
			_prefabItemLists[itemPrefabIndex].activeItems.AddLast(_item);
			
			if (DebugEvents)
				Debug.Log("Instantiated a new item " + _go.name + " at frame: " + Time.frameCount);
		}
		
		// move the item to active parent transform.
		// this is mainly just for visual reference in the editor
		SetItemParentTransform(_item, _activeParentTransform);
		
		// set the state to active
		_item.State = SwarmItem.STATE.Active;
		
		// if the prune timer is runnning
		if (_prefabItemLists[itemPrefabIndex].inactivePruneTimeLeft > 0)
		{		
			// if the inactive item count dropped below the threshold
			if (((float)_prefabItemLists[itemPrefabIndex].inactiveItems.Count / (float)_prefabItemLists[itemPrefabIndex].itemCount) < itemPrefabs[itemPrefabIndex].inactiveThreshold)
			{
				if (DebugEvents)
					Debug.Log("Dropped below inactive threshold [" + (itemPrefabs[itemPrefabIndex].inactiveThreshold * 100) + "%] for " + itemPrefabs[itemPrefabIndex].prefab.name + " list before timer expired. Stopping prune timer at frame: " + Time.frameCount);
				
				// turn the prune timer off
				_prefabItemLists[itemPrefabIndex].inactivePruneTimeLeft = 0;
			}
		}
		
		return _item;
	}
	
	/// <summary>
	/// Moves a SwarmItem from the active list to the inactive list and changes its parent transform 
	/// </summary>
	/// <param name="item">The SwarmItem to deactivate</param>
	public virtual void DeactiveItem(SwarmItem item)
	{
		// remove from the active linked list
		_prefabItemLists[item.PrefabIndex].activeItems.Remove(item);
		// push onto the inactive stack
		_prefabItemLists[item.PrefabIndex].inactiveItems.Push(item);
		
		// move the item to the inactive parent transform.
		// this is mainly just for visual reference in the editor
		SetItemParentTransform(item, _inactiveParentTransform);
		
		if (DebugEvents)
			Debug.Log("Deactivated " + item.name + " at frame: " + Time.frameCount);

		// if the prune timer is not currently running and we actually want to prune
		if (_prefabItemLists[item.PrefabIndex].inactivePruneTimeLeft == 0 && itemPrefabs[item.PrefabIndex].inactivePrunePercentage > 0)
		{
			// if the inactive item count exceeds the threshold
			if (((float)(_prefabItemLists[item.PrefabIndex].inactiveItems.Count) / (float)_prefabItemLists[item.PrefabIndex].itemCount) >= itemPrefabs[item.PrefabIndex].inactiveThreshold)
			{
				if (DebugEvents)
					Debug.Log("Inactive threshold [" + (itemPrefabs[item.PrefabIndex].inactiveThreshold * 100) + "%] reached for " + itemPrefabs[item.PrefabIndex].prefab.name + " list. Starting prune timer [" + itemPrefabs[item.PrefabIndex].inactivePruneTimer + " seconds] at frame: " + Time.frameCount);
				
				// if the prune timer is set to expire immediately
				if (itemPrefabs[item.PrefabIndex].inactivePruneTimer == 0)
				{
					// don't wait for a countdown, just prune immediately
					PruneList(item.PrefabIndex, itemPrefabs[item.PrefabIndex].inactivePrunePercentage);
				}
				else
				{
					// turn the prune timer on
					_prefabItemLists[item.PrefabIndex].inactivePruneTimeLeft = itemPrefabs[item.PrefabIndex].inactivePruneTimer;
				}
			}
		}
	}
	
	/// <summary>
	/// Creates a new gameobject for the SwarmItem and initializes it 
	/// </summary>
	/// <param name="itemPrefabIndex">The index of the prefab to use (type of SwarmItem)</param>
	/// <returns>Returns the newly created SwarmItem</returns>
	protected virtual SwarmItem InstantiateItem(int itemPrefabIndex)
	{
		SwarmItem item;
		
		// instantiate
		_go = (GameObject)Instantiate(itemPrefabs[itemPrefabIndex].prefab);
		// change the name of the gameobject with an index and take off the 'Clone' postfix
		_go.name = "[" + _itemCount.ToString("0000") + "] " + _go.name.Replace("(Clone)", "");
		
		// get the SwarmItem component from the gameobject
		item = (SwarmItem)_go.GetComponent(typeof(SwarmItem));
		// initialize the SwarmItem
		item.Initialize(this, itemPrefabIndex, DebugEvents);
		
		// increase the item count for this prefab
		_prefabItemLists[itemPrefabIndex].itemCount++;
		
		// increment the total item count for this manager
		_itemCount++;
		
		return item;
	}
	
	/// <summary>
	/// Moves a SwarmItem to the active or inactive transforms. 
	/// This is mainly used a visual aid in the editor to see which items are active or inactive
	/// </summary>
	/// <param name="item">The SwarmItem to move</param>
	/// <param name="parentTransform">The parent transform to move to</param>
	private void SetItemParentTransform(SwarmItem item, Transform parentTransform)
	{
		// reparent this item's transform
		item.ThisTransform.parent = parentTransform;
		
		// reset the position, rotation, and scale to unit values
		item.ThisTransform.localPosition = Vector3.zero;
		item.ThisTransform.localRotation = Quaternion.identity;
		item.ThisTransform.localScale = Vector3.one;
		
		// if the position, rotation, or scale need to be changed after reparenting, do it in the
		// item's OnSetParentTransform method
		item.OnSetParentTransform();
	}
	
	/// <summary>
	/// This function is called every frame. It will iterate through each SwarmItem type's active list,
	/// calling FrameUpdate on each of the items. This method should be called from a central Update() Mono
	/// method, or you can add an Update() method here and call it.
	/// </summary>
	public virtual void FrameUpdate()
	{
		// iterate through each SwarmItem type
		for (int i=0; i<_prefabItemLists.Length; i++)
		{
			// only bother if the active list has some items
			if (_prefabItemLists[i].activeItems.Count > 0)
			{
				// we don't iterate through the active list using foreach here
				// because there would be errors if the item was killed in its FrameUpdate method.
				// instead we manually move to the next linkedlist node
				
				LinkedListNode<SwarmItem> item;
				LinkedListNode<SwarmItem> nextItem;
				
				item = _prefabItemLists[i].activeItems.First;
				
				// while there are items left to process
				while (item != null)
				{
					// cache the next item in case this item is killed in its FrameUpdate
					nextItem = item.Next;
					
					// update and move to the next item
					item.Value.FrameUpdate();
					item = nextItem;
				}
			}
			
			// if this list has its prune timer turned on
			if (_prefabItemLists[i].inactivePruneTimeLeft > 0)
			{
				// decrement the prune timer
				_prefabItemLists[i].inactivePruneTimeLeft -= Time.deltaTime;
				
				// if the timer has expired
				if (_prefabItemLists[i].inactivePruneTimeLeft <= 0)
				{
					// prune the list
					PruneList(i, itemPrefabs[i].inactivePrunePercentage);
				}
			}
		}
	}
	
	/// <summary>
	/// Removes inactive items from the list after the inactive item count exceeds a threshold and 
	/// no new items are activated from the list before the prune timer countdown expires. Alternatively,
	/// you could call this manually to free up memory at any time.
	/// </summary>
	/// <param name="itemPrefabIndex">
	/// The index of the list to prune
	/// </param>
	/// <param name="prunePercentage">
	/// The amount (relative to the number of inactive items) to prune from the inactive list.
	/// </param>
	public void PruneList(int itemPrefabIndex, float prunePercentage)
	{
		// turn off the prune timer
		_prefabItemLists[itemPrefabIndex].inactivePruneTimeLeft = 0;
		
		// get the number of items to prune based on the prune percentage.
		// the amount is a percentage of the inactive items, not the total item count for this list
		int pruneCount = Mathf.FloorToInt(prunePercentage * (float)_prefabItemLists[itemPrefabIndex].inactiveItems.Count);
		SwarmItem item;
		
		if (DebugEvents)
			Debug.Log("Pruning " + pruneCount + " items [" + (itemPrefabs[itemPrefabIndex].inactivePrunePercentage*100) + "% of " + _prefabItemLists[itemPrefabIndex].inactiveItems.Count + "] from inactive " + itemPrefabs[itemPrefabIndex].prefab.name + " list at frame: " + Time.frameCount);
		
		// prune each item
		while (pruneCount > 0)
		{
			// pop an item from the inactive stack
			item = (SwarmItem)_prefabItemLists[itemPrefabIndex].inactiveItems.Pop();
			
			// call the overloaded PreDestroy function to let the inherited objects
			// free any memory
			item.PreDestroy();
			
			if (DebugEvents)
				Debug.Log("Destroyed " + item.name + " at frame: " + Time.frameCount);
			
			// destroy the item
			Destroy(item.gameObject);
			item = null;
			
			// decrement this list's item count and the manager's total item count
			_prefabItemLists[itemPrefabIndex].itemCount--;
			_itemCount--;
			
			// move to the next item to prune
			pruneCount--;
		}		
	}
}
