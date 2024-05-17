using UnityEngine;
using UnityEngine.Events;

//Place this script on the same object of the weapon trigger
public class WeaponEvents : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Collider> event_weaponHitsSomething;

    private void OnEnable()
    {
        if (event_weaponHitsSomething == null)
            event_weaponHitsSomething = new UnityEvent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {

        event_weaponHitsSomething.Invoke(other);
    }
}
