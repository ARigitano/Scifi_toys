
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using TMPro;

public class Prop : UdonSharpBehaviour
{
    private bool _isDropped = false; //Has the prop been dropped after having been picked up?
    public bool isSnapped = false; //Has the prop entered a snapzone?
    private bool _isActivated = false; //Has the prop came to life?
    private VRCPickup _pickup; //The VRC Pickup component of this prop.

    [SerializeField]
    private GameObject _environment; //An environment surrounding the prop.
    [SerializeField]
    private GameObject _base; //A collider base for buildings.

    [SerializeField]
    private SceneSizeIncreaserManager _sizeManager; //Reference to this scene's size scaler.
    private bool _isScaleCounted = false; //Has this prop been already counted for the world scaling event?

    [SerializeField]
    private GameObject _carpetHighlight; //Highlight object that shows where to drop the prop.

    [SerializeField]
    private AudioSource _successSound; //The sound played when the prop is successfully placed on the carpet.

    [SerializeField]
    private TextMeshPro _textTableNb; //The text that shows the number of building that have been placed.

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
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SuperActivate");
            }
        }
    }

    //Quick fix for event that was only local
    public void SuperActivate()
    {
        _sizeManager.ScaleWorldActivate();

        //A mettre coté SceneSizeIncreaseManager?
        _successSound.Play();
        _textTableNb.text = _sizeManager.nbProps.ToString() + "/5";
        _isScaleCounted = true;
    }

    //Enables the environment for this prop
    public virtual void EnableEnvironment()
    {
        if (_environment != null)
        {
            _environment.SetActive(true);
        }

        VRCPickup pickup = this.gameObject.GetComponent<VRCPickup>();
        Destroy(pickup);

        _base.SetActive(true);

        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        Destroy(rb);
        //rb.isKinematic = true;

        BoxCollider collider = this.gameObject.GetComponent<BoxCollider>();
        Destroy(collider);
        //collider.enabled = false;

    }
}
