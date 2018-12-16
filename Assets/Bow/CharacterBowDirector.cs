using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBowDirector : WeaponDirector {

    public Bow bow;
    private Character character;

    private void Start() {
        character = GetComponent<Character>();
    }

    public override void OnPrepare() {
        // Before stick to the bowstring, we should instantiate arrow,
        // And stick it to the right hand
        // ... TODO

        // Then bowstring should be sticked at the end of reload animation
        // ... before doing that here, we first need to figure out when animation is stop playing
        // or when some event occur, so for now we stick only after onAim is called
    }

    public override void OnAim() {
        RightHand rightHand = character.GetComponentInChildren<RightHand>();
        bow.StickTo(rightHand.transform);
    }

    public override void OnShot() {
        bow.Release();
    }

}
