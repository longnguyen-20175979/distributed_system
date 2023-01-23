using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHidden : MonoBehaviour {
    [SerializeField] bool isCloud;
    Vector3 tempPosPlayer;
    float x;
    Vector3 camTarget;
    float posMinX;
    private void Start() {
        posMinX = transform.position.x - GetComponent<Camera>().orthographicSize / 2;
        camTarget.y = transform.position.y;
    }
    private void FixedUpdate() {
        if (isCloud) {
            if (tempPosPlayer != PlayerMovement.instance.transform.position) {
                x = Mathf.Clamp(PlayerMovement.instance.transform.position.x, posMinX, 1000);
                tempPosPlayer = PlayerMovement.instance.transform.position;
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, camTarget.y, -10), 6f * Time.deltaTime);
        }
    }
}
