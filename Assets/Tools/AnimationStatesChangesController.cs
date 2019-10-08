using System.Collections.Generic;
using UnityEngine;

namespace Tools {
    public class AnimationStatesChangesController {
        private readonly List<AnimationStateChangesListener>
            animatorChangesListeners = new List<AnimationStateChangesListener>();

        public AnimationStatesChangesController(Animator animator) {
            var animationStateChangesObservers = animator.GetBehaviours<AnimationStateChangesObserver>();
            if (animationStateChangesObservers == null) {
                return;
            }

            if (animationStateChangesObservers.Length !=
                animator.runtimeAnimatorController.animationClips.Length) {
                Debug.LogWarning("Potentially not all states have changes observer attached, states: " +
                                 animationStateChangesObservers.Length + " clips: " +
                                 animator.runtimeAnimatorController.animationClips.Length);
            }

            foreach (var changesObserver in animationStateChangesObservers) {
                changesObserver.OnStateEnterEvent += (_, stateInfo, layerIndex) => {
                    var listeners = new List<AnimationStateChangesListener>(animatorChangesListeners);
                    foreach (var changesListener in listeners) {
                        changesListener.OnStateEntered(animator, stateInfo, layerIndex);
                    }
                };

                changesObserver.OnStateExitEvent += (_, stateInfo, layerIndex) => {
                    var listeners = new List<AnimationStateChangesListener>(animatorChangesListeners);
                    foreach (var changesListener in listeners) {
                        changesListener.OnStateExited(animator, stateInfo, layerIndex);
                    }
                };
            }
        }

        public void AddAnimationStateChangesListener(AnimationStateChangesListener listener) {
            animatorChangesListeners.Add(listener);
        }

        public void RemoveAnimationStateChangesListener(AnimationStateChangesListener listener) {
            animatorChangesListeners.Remove(listener);
        }
    }
}