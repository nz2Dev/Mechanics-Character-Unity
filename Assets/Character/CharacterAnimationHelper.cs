using System;
using Tools;
using UnityEngine;

public class CharacterAnimationHelper {
    
    private readonly AnimationStateChangesListener releasingToPreparationStateChangesListener;
    private readonly Character character;

    private bool aimedSpineModeFrozen;
    private bool lastAimedSpineModeValue;

    public CharacterAnimationHelper(Character character) {
        this.character = character;
        releasingToPreparationStateChangesListener = 
            new ReleasingToPreparingStateChangesListener(OnStateChanged);
    }

    public void SetAimedSpineMode(bool aimedMode) {
        if (aimedSpineModeFrozen) {
            lastAimedSpineModeValue = aimedMode;
            return;
        }
        
        character.SetAimedSpineMode(aimedMode);
    }
    
    public void FreezeAimedSpineModeUntilAttackReleased(bool aimedSpineMode) {
        if (aimedSpineModeFrozen) {
            return;
        }
        
        var statesChangesController = character.GetAnimationStatesChangesController();
        statesChangesController.AddAnimationStateChangesListener(releasingToPreparationStateChangesListener);
        
        aimedSpineModeFrozen = true;
        lastAimedSpineModeValue = aimedSpineMode;
        character.SetAimedSpineMode(aimedSpineMode);
    }

    private void OnStateChanged() {
        var statesChangesController = character.GetAnimationStatesChangesController();
        statesChangesController.RemoveAnimationStateChangesListener(releasingToPreparationStateChangesListener);

        aimedSpineModeFrozen = false;
        character.SetAimedSpineMode(lastAimedSpineModeValue);
    }

    private class ReleasingToPreparingStateChangesListener : AnimationStateChangesListener {

        private readonly Action callback;

        internal ReleasingToPreparingStateChangesListener(Action callback) {
            this.callback = callback;
        }

        public override void OnStateExited(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsName("Upper.Releasing")) {
                callback();
            }
        }
    }

}