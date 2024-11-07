
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//Object that increases the scale of the world when used
public class SceneSizeIncreaser : Prop
{
    private bool _isUsed = false; //Has the object already been used to increase the size of the room?

    [SerializeField]
    private float _targetScale = 100f; //Scale of the world after object is used
    [SerializeField]
    private Transform _scalableWorld; //The world which size increases

    [SerializeField]
    private Prop[] _propsEnvironement; //Props that need their environments to be activated

    /*public override void OnPickupUseDown()
    {
        base.OnPickupUseDown();
        _scalableWorld.localScale = new Vector3(_targetScale, _targetScale, _targetScale);
    }*/

    public override void OnPickup()
    {
        base.OnPickup();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ScaleWorld");
    }

    //Makes the world giant when picked up
    public void ScaleWorld()
    {
        _scalableWorld.localScale = new Vector3(_targetScale, _targetScale, _targetScale);

        foreach(Prop prop in _propsEnvironement)
        {
            prop.EnableEnvironment();
        }
    }
}
