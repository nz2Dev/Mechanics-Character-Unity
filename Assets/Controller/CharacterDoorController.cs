using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDoorController : MonoBehaviour {

    // Just for test purpose, we will inject known door in this game object, but we should
    // figure out how to observe possibility for player to open the dors.
    // We can use Physics.SphericCast to get all the nearest door, and show to the user, that hi can click the button, to open that door. (Catche the handle)
    public Door door;

    private DoorOpeningDirector doorOpeningDirector;
    
	void Start () {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player) {
            doorOpeningDirector = player.GetComponent<DoorOpeningDirector>();
        } else {
            Debug.LogWarning("No door opening director found");
        }
	}
	
	void Update () {
	    if (doorOpeningDirector) {
            if (Input.GetKeyDown(KeyCode.F)) {
                doorOpeningDirector.OpenDoor(door);
            }
        }	
	}

}
