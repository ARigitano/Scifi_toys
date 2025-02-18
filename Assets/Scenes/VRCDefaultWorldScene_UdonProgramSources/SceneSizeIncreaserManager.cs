
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SceneSizeIncreaserManager : UdonSharpBehaviour
{
    private bool _isUsed = false; //Has the object already been used to increase the size of the room?

    [SerializeField]
    private float _targetScale = 100f; //Scale of the world after object is used.
    [SerializeField]
    private Transform _scalableWorld; //The world which size increases
    private bool _isScaling = false; //Is the world scaling?
    private bool _hasScaled = false; //Has the world finished scaling?
    [SerializeField]
    private float _scaleSpeed = 1f; //The speed at which the world scales.

    [SerializeField]
    private Prop[] _propsEnvironement; //Props that need their environments to be activated.
    [SerializeField]
    private GameObject _tools; //The tools that will fall from the sky.
    [SerializeField]
    private GameObject _bigMirror; //The main mirror of the room.
    [SerializeField]
    private GameObject _tableText; // The text on the table.
    [SerializeField]
    private GameObject _snapArea; //The carpet area where buildings snap.

    [UdonSynced]
    public int nbProps = 0; //Number of props that have been put on snapping surface.
    [SerializeField]
    private int _nbPropsActivate = 4; //Number of props that need to be put on snapping surface to activate the world scaler.

    [SerializeField]
    private AudioSource _shrinkingSound; //The sound for when the world changes size.

    private void Update()
    {
        if (_isScaling && !_hasScaled)
        {
            float _scalingNb = Time.deltaTime * _scaleSpeed;

            if (_scalableWorld.localScale.x <= _targetScale)
            {   
                _scalableWorld.localScale += new Vector3(_scalingNb, _scalingNb, _scalingNb);
            }
            else
            {
                foreach (Prop prop in _propsEnvironement)
                {
                    prop.EnableEnvironment();
                }

                _tools.SetActive(true);

                _hasScaled = true;
            }
        }
    }


    //Activates the world scaling
    public void ScaleWorldActivate()
    {
        nbProps++;

        if (nbProps >= _nbPropsActivate)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ScaleWorld");
        }
    }

    //Makes the world giant when picked up
    public void ScaleWorld()
    {
        _isScaling = true;
        _bigMirror.SetActive(false);
        _tableText.SetActive(false);
        _snapArea.SetActive(false);
        _shrinkingSound.Play();

        /*if (!_hasScaled)
        {
            _scalableWorld.localScale = new Vector3(_targetScale, _targetScale, _targetScale);

            foreach (Prop prop in _propsEnvironement)
            {
                prop.EnableEnvironment();
            }

            _hasScaled = true;
        }*/
    }
}
