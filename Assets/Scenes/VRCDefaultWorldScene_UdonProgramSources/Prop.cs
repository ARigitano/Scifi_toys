
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[RequireComponent(typeof(VRCPickup))]
[RequireComponent(typeof(Rigidbody))]
public class Prop : UdonSharpBehaviour
{
    private bool _isDropped = false; //Has the prop been dropped after having been picked up?
    public bool isSnapped = false; //Has the prop entered a snapzone?
    private bool _isActivated = false; //Has the prop came to life?

    public override void OnDrop()
    {
        base.OnDrop();
        _isDropped = true;
        Debug.Log("dropped");
    }

    //Sends the event to activate the prop accross the network
    public void ActivateNetwork()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Activate");
    }

    //Makes the prop come to life
    public void Activate()
    {
        if (_isDropped && isSnapped)
        {
            _isActivated = true;
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
            Debug.Log("ACTIVATED");
        }
    }
}
