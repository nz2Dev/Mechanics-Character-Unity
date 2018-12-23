using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowstringTester : MonoBehaviour {

    public Bow bow;
    public GameObject handler;

    [ContextMenu("Stick")]
    public void Stick() {
        bow.StickBowstringTo(handler.transform);
    }

    [ContextMenu("Release")]
    public void Release() {
        bow.Release();
    }

}
