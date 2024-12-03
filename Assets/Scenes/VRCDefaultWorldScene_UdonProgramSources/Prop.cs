
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
    private VRCPickup _pickup; //The VRC Pickup component of this prop.

    [SerializeField]
    private GameObject _environment; //An environment surrounding the prop.

    [SerializeField]
    private SceneSizeIncreaserManager _sizeManager; //Reference to this scene's size scaler.
    private bool _isScaleCounted = false; //Has this prop been already counted for the world scaling event?

    [SerializeField]
    private GameObject _carpetHighlight; //Highlight object that shows where to drop the prop.

    public override void OnPickup()
    {
        base.OnPickup();
        _carpetHighlight.SetActive(true);
    }


    public override void OnDrop()
    {
        base.OnDrop();
        _carpetHighlight.SetActive(false);
        _isDropped = true;
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

            if (_sizeManager != null && !_isScaleCounted)
            {
                _sizeManager.ScaleWorldActivate();
                _isScaleCounted = true;
            }

            //_pickup.enabled = false; Causes a bug for now
        }
    }

    //Enables the environment for this prop
    public void EnableEnvironment()
    {
        if (_environment != null)
        {
            VRCPickup pickup = this.gameObject.GetComponent<VRCPickup>();
            pickup.pickupable = false;
            _environment.SetActive(true);
        }
            
    }
}
