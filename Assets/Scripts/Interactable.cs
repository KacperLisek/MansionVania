using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface to group all interactables together and provide common functions
public interface Interactable
{
    public void OnInteraction();

    public int reward();
}
