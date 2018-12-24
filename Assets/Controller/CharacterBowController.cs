using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBowController : MonoBehaviour {

    private CharacterBowDirector characterBowDirector;

    private void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        characterBowDirector = player.GetComponent<CharacterBowDirector>();
    }

    // Update is called once per frame
    void Update() {
        // Right DOWN
        if (Input.GetMouseButtonDown(1)) {
            // Together with looking at the point when right mouse click
            // We would also prepare our weapon
            characterBowDirector.Prepare(true);
        }

        // Right UP
        if (Input.GetMouseButtonUp(1)) {
            // Seems that attack and move states should be handled at once 
            characterBowDirector.Prepare(false);
        }

        // Left DOWN
        if (Input.GetMouseButtonDown(0)) {
            characterBowDirector.Aim(true);
        }

        // Left UP
        if (Input.GetMouseButtonUp(0)) {
            characterBowDirector.Aim(false);
        }
    }

}
