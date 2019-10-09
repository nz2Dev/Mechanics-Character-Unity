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
                animationHelper.FreezeAimedSpineModeUntilAttackReleased(true);
                animationHelper.SetAimedSpineMode(false);
            } else {
                animationHelper.SetAimedSpineMode(true);
            }
        }
    }

    public void Prepare(bool prepare) {
        character.PrepareAttack(prepare);
        animationHelper.SetAimedSpineMode(character.IsAttackAimed() && prepare);
    }

    public void OnCatchArrow() {
        var rightHand = character.GetComponentInChildren<RightHand>();
        arrowInUse = Instantiate(arrowPrefab, rightHand.transform);
        var arrowTransform = arrowInUse.transform;
        arrowTransform.localPosition = arrowOrientation.position;
        arrowTransform.localRotation = arrowOrientation.rotation;
    }

    public void OnCatchBowstring() {
        var rightHand = character.GetComponentInChildren<RightHand>();
        arrowInUse.collisionFreeObjects = new[] {character.GetComponentInChildren<LeftHand>().GetComponent<Collider>()};
        bow.StickBowstringTo(rightHand.transform);
        bow.LoadArrow(arrowInUse);
    }

    public void OnReleaseBowstringAndArrow() {
        bow.Release();
    }

    public void OnReleaseBowstring() {
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

    public void OnReleaseArrow() {
        // it should be called from animation when arrow is loaded back into quiver
        // so probable its better to rename it appropriately
        if (arrowInUse) {
            Destroy(arrowInUse.gameObject);
        }
    }

}