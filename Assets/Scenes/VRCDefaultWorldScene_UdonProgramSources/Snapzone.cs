
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Snapzone : UdonSharpBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Prop>() != null)
        {
            other.gameObject.GetComponent<Prop>().isSnapped = true;
            other.gameObject.GetComponent<Prop>().ActivateNetwork();
        }
    }
}
