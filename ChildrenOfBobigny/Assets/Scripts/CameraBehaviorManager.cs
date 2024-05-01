using UnityEngine;
using Cinemachine;

public class CameraBehaviorManager : MonoBehaviour
{
    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
