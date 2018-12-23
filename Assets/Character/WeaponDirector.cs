using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponDirector : MonoBehaviour {

    public abstract void OnPrepare();
    public abstract void OnAim();
    public abstract void OnHide();
    public abstract void OnShot();
    public abstract void OnReverseAim();

}
