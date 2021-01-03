/* 
    ------------------- Code Monkey -------------------
    
    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey.MonoBehaviours {

    /*
     * Easy set up for CameraFollow, it will follow the transform with zoom
     * */
    public class CameraFollowSetup : MonoBehaviour {

        [SerializeField] private CameraFollow cameraFollow = default;
        [SerializeField] private Transform followTransform = default;
        [SerializeField] private float zoom = default;

        private void Start() {
            if (followTransform == null) {
                Debug.LogError("followTransform is null! Intended?");
                cameraFollow.Setup(() => Vector3.zero, () => zoom);
            } else {
                cameraFollow.Setup(() => followTransform.position, () => zoom);
            }
        }
    }

}