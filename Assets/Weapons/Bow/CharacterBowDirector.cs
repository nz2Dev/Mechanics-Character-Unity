using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterBowDirector : MonoBehaviour {

    public Bow bow;
    public Arrow arrowPrefab;
    public Transform arrowOrientation;

    private Character character;
    private CharacterAnimationHelper animationHelper;

    private Arrow arrowInUse;

    private void Start() {
        character = GetComponent<Character>();
        animationHelper = new CharacterAnimationHelper(character);
    }

    public void Aim(bool aim) {
        character.AimAttack(aim);
        if (character.IsAttackPrepared()) {
            if (!aim) {
                animationHelper.FreezeAimedSpineModeUntilAttackReleased();
            } else {
                animationHelper.SetAimedSpineMode(true);
            }
        }
    }

    public void Prepare(bool prepare) {
        character.PrepareAttack(prepare);
        animationHelper.SetAimedSpineMode(character.IsAttackAimed() && prepare);
        
        var ctb = character.GetComponent<Animator>().GetBehaviour<CustomTransitionBehaviour>();
        if (!character.IsAttackAimed()) {
            if (!prepare) {
                ctb.TransitsToReverse();
            } else {
                ctb.ResetTransitToFlag();
            }
        }
    }

    public void OnTakeArrow() {
        var rightHand = character.GetComponentInChildren<RightHand>();
        arrowInUse = Instantiate(arrowPrefab, rightHand.transform);
        var arrowTransform = arrowInUse.transform;
        arrowTransform.localPosition = arrowOrientation.position;
        arrowTransform.localRotation = arrowOrientation.rotation;
    }

    public void OnGrabBowstring() {
        var rightHand = character.GetComponentInChildren<RightHand>();
        arrowInUse.collisionFreeObjects = new[] {character.GetComponentInChildren<LeftHand>().GetComponent<Collider>()};
        bow.StickBowstringTo(rightHand.transform);
        bow.LoadArrow(arrowInUse);
    }

    public void OnReleaseLoadedBowstring() {
        bow.Release();
    }

    public void OnDropBowstring() {
        arrowInUse = bow.UnloadArrow();
        
        if (arrowInUse) {
            var arrowTransform = arrowInUse.transform;
            var rightHand = character.GetComponentInChildren<RightHand>();
            arrowTransform.parent = rightHand.transform;
            arrowTransform.localPosition = arrowOrientation.position;
            arrowTransform.localRotation = arrowOrientation.rotation;
        }

        bow.UnstickBowstring();
    }

    public void OnHideArrowToQuiver() {
        // well, if this animation is plying, then arrow should be there!
        // error otherwise
        Destroy(arrowInUse.gameObject);
    }

}