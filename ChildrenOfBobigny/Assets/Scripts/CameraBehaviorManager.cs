using UnityEngine;
using Cinemachine;

public class CameraBehaviorManager : Singleton<CameraBehaviorManager>
{
    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
