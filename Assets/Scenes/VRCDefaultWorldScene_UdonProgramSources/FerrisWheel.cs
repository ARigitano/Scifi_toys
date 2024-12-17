
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FerrisWheel : Prop
{
    [SerializeField]
    private GameObject _wheel; //The part of the ferris wheel that turns.
    [SerializeField]
    private float _turningSpeed = 20f; //The speed at which the wheel turns.
    [SerializeField]
    private GameObject[] _seats; //The seats of the wheel.

    private bool _isTurning = false; //Is the wheel turning?

    public override void EnableEnvironment()
    {
        base.EnableEnvironment();

        _isTurning = true;
    }

    private void Update()
    {
        if (_isTurning)
        {
            _wheel.transform.Rotate(_wheel.transform.right, Time.deltaTime * _turningSpeed * -1);

            foreach (GameObject seat in _seats)
            {
                seat.transform.LookAt(seat.transform.position + Vector3.down, transform.forward);
                seat.transform.Rotate(-90f, 0f, 0f);
            }
        }
    }
}
