using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// For items that get placed into chests, they need some kind of functionalty to turn off
/// colliders / rigidbody / etc so that they can behave while 'locked in' the chest. This component
/// takes care of that. The actual functionality of what to do in/out of chests is left generic,
/// because that will vary widely from item to item.
/// </summary>
public class ChestItem : MonoBehaviour
{
    public UnityEvent onInitInChest;
    public UnityEvent onExitChest;
}
