using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameResource
{
    public string ID;
    public string Description;
}

[Serializable]
public class ResourceAmount
{
    public string ID;
    public int amount;
    public int randomMod = 1;
}

public class ResorceManager : MonoBehaviour
{
    public List<GameResource>  resources;
}
