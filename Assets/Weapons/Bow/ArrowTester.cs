using UnityEngine;

public class ArrowTester : MonoBehaviour
{

    public Arrow arrow;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Arrow arrowClone = Instantiate(arrow, transform.position, transform.rotation);
            arrowClone.Launch();
        }   
    }

}
