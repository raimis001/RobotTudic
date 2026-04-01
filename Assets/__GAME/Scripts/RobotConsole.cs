using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConsoleSlot
{
    public Transform slotTransform;
    public bool isOccupied;
}

public class RobotConsole : MonoBehaviour
{
    [SerializeField]
    List<ConsoleSlot> slots;
}
