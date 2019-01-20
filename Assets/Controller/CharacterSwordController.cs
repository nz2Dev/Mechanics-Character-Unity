using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwordController : MonoBehaviour {

    private CharacterSwordDirector characterSwordDirector;

    private void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        characterSwordDirector = player.GetComponent<CharacterSwordDirector>();
    }
    
    void Update () {
        // Right DOWN
        if (Input.GetMouseButtonDown(1)) {
            // Together with looking at the point when right mouse click
            // We would also prepare our weapon
            characterSwordDirector.Prepare(true);
        }

        // Right UP
        if (Input.GetMouseButtonUp(1)) {
            // Seems that attack and move states should be handled at once 
            characterSwordDirector.Prepare(false);
        }

        // Left DOWN
        if (Input.GetMouseButtonDown(0)) {
            characterSwordDirector.Aim(true);
        }

        // Left UP
        if (Input.GetMouseButtonUp(0)) {
            characterSwordDirector.Aim(false);
        }
    }
}
