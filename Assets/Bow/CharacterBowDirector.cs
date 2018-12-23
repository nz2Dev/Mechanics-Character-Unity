using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterBowDirector : MonoBehaviour {

    public Bow bow;
    public Arrow arrowPrefab;
    public Transform arrowOrientation;

    private Character character;

    private Arrow arrowInUse;

    private void Start() {
        character = GetComponent<Character>();
    }

    public void OnCatchArrow() {
        RightHand rightHand = character.GetComponentInChildren<RightHand>();
        arrowInUse = Instantiate(arrowPrefab, rightHand.transform);
        arrowInUse.transform.localPosition = arrowOrientation.position;
        arrowInUse.transform.localRotation = arrowOrientation.rotation;
    }

    public void OnCatchBowstring() {
        RightHand rightHand = character.GetComponentInChildren<RightHand>();
        bow.StickBowstringTo(rightHand.transform);
    }

    public void OnReleaseBowstringAndArrow() {
        bow.Release();
        arrowInUse.Launch(character.transform.forward);
        arrowInUse.transform.parent = null;
        StartCoroutine(PostLaunch());
    }

    public void OnReleaseBowstring() {
        bow.UnstickBowstring();
    }

    public void OnReleaseArrow() {
        Destroy(arrowInUse.gameObject);
    }

    private IEnumerator PostLaunch() {
        yield return new WaitForSeconds(0.5f);
        arrowInUse.Launched();
        arrowInUse = null;
    }

}
