using UnityEngine;

public class Bow : MonoBehaviour {
    
    private const float ReleasedThreshold = 0.1f;

    private Transform manipulatorHolderTransform;
    private Vector3 manipulatorStartPosition;
    private Arrow loadedArrow;

    private Vector3 returnDirection;
    private bool releasing;
    private bool sticking;

    [SerializeField] private Transform head;
    [SerializeField] private Manipulator manipulator;
    [SerializeField] private float bowstringStrength = 1;
    
    public void StickBowstringTo(Transform holder) {
        manipulatorStartPosition = manipulator.transform.localPosition;
        manipulatorHolderTransform = holder;
        releasing = false;
        sticking = true;
    }

    public void LoadArrow(Arrow arrow)
    {
        arrow.transform.parent = manipulator.transform;
        loadedArrow = arrow;
    }

    public void UnstickBowstring() {
        returnDirection = manipulatorStartPosition - manipulator.transform.localPosition;
        releasing = true;
        sticking = false;
    }

    public void Release() {
        returnDirection = manipulatorStartPosition - manipulator.transform.localPosition;
        releasing = true;
        sticking = false;

        if (loadedArrow) {
            loadedArrow.Launch();
            loadedArrow = null;
        }
    }

    private void Update() {
        if (sticking) {
            manipulator.transform.position = manipulatorHolderTransform.position;
            
            if (loadedArrow) {
                var manipulatorTransform = manipulator.transform;
                var manipulatorTransformPosition = manipulatorTransform.position;
                var bowHeadDirection = head.position - manipulatorTransformPosition;
                
                // hotfix. moving arrow forward a bit, because their center is in the middle
                loadedArrow.transform.position = manipulatorTransformPosition + bowHeadDirection.normalized / 2;
                loadedArrow.transform.rotation = Quaternion.LookRotation(bowHeadDirection, manipulatorTransform.up);
            }
        }
        
        if (releasing) {
            manipulator.transform.localPosition += Time.deltaTime * bowstringStrength * returnDirection;
            
            if (Vector3.Distance(manipulator.transform.localPosition, manipulatorStartPosition) < ReleasedThreshold) {
                manipulator.transform.localPosition = manipulatorStartPosition;
                releasing = false;
            }
        }
    }
    
}
