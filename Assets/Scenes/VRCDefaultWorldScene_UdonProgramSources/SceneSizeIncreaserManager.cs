
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SceneSizeIncreaserManager : UdonSharpBehaviour
{
    private bool _isUsed = false; //Has the object already been used to increase the size of the room?

    [SerializeField]
    private float _targetScale = 100f; //Scale of the world after object is used
    [SerializeField]
    private Transform _scalableWorld; //The world which size increases
    private bool _hasResized = false; //Was the prop used to resize the world?

    [SerializeField]
    private Prop[] _propsEnvironement; //Props that need their environments to be activated

    private int _nbProps = 0; //Number of props that have been put on snapping surface.
    [SerializeField]
    private int _nbPropsActivate = 4; //Number of props that need to be put on snapping surface to activate the world scaler.


    //Activates the world scaling
    public void ScaleWorldActivate()
    {
        _nbProps++;

        if (_nbProps >= _nbPropsActivate)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ScaleWorld");
            Debug.Log(_nbProps + " " + _nbPropsActivate);
            Debug.Log(_nbProps >= _nbPropsActivate);
        }
    }

    //Makes the world giant when picked up
    public void ScaleWorld()
    {
        if (!_hasResized)
        {
            _scalableWorld.localScale = new Vector3(_targetScale, _targetScale, _targetScale);

            foreach (Prop prop in _propsEnvironement)
            {
                prop.EnableEnvironment();
            }

            _hasResized = true;
        }
    }
}
