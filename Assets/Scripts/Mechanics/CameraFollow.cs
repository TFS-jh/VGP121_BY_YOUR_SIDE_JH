using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;
    [SerializeField] private float minYPos;
    [SerializeField] private float maxYPos;

    [SerializeField] private Transform target; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (target == null) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null){
                Debug.Log("hey stupid you got no target assigned AND no player game object go fix that");
                return;
            }
            target = player.transform;
        }
    }

    //Inputs being polled in update - Physics generally are applied in FixedUpdate - and cam movement is done in late update

    //Update is within the computer tick rate
    //Fixed Update is a fixed rate at which your game updates
    //Late Update happens as the last possible update for that frame

    // Update is called once per frame
    void LateUpdate()
    {
        //early return - if we don't have a target, we can't follow anything so we shouldn't do anything
        if (target == null) return;

        //Store our current position
        Vector3 currentPos = transform.position;

        //Update our X pos to be identical to our target's X pos - with b clamped between min and max values
        currentPos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);
        currentPos.y = Mathf.Clamp(target.position.y, minYPos, maxYPos);

        Vector3 offset = target.position - transform.position;

        //Apply the position back to the camera
        if (offset.sqrMagnitude > (8 * 8))
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPos, 64f * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPos, 8f * Time.deltaTime);
        }
    }
}
