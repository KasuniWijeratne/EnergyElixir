using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] SpriteRenderer backgroundImage;

    // Camera movement & zoom speed
    [SerializeField] float moveSpeed = 50f;
    [SerializeField] float zoomSpeed = 25.0f;

    // Smooth zoom transition
    [SerializeField] float smoothTime = 0.3f;
    private float targetZoom;
    private float zoomVelocity = 0.0f;

    private float relativeRotation = 15.0f;

    private Bounds bounds;
    private float scale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

        bounds = backgroundImage.sprite.bounds;
        scale = backgroundImage.transform.localScale.x;
        if (cam.orthographic) {
            targetZoom = cam.orthographicSize;
        }
        else {
            targetZoom = cam.fieldOfView;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        ZoomCamera();
    }

    private void ZoomCamera() {
        // Camera zoom inputa
        float zoomChange = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        targetZoom -= zoomChange;
        targetZoom = Mathf.Clamp(targetZoom, 18, 35); // Adjust the min and max zoom limits as necessary

        // Smooth zoom transition
        if (cam.orthographic) {
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothTime);
        }
        else {
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetZoom, ref zoomVelocity, smoothTime);
        }
    }

    private void MoveCamera() {
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;


        

        // Get the vertical field of view (which is the default field of view in Unity)
        float verticalFOV = cam.fieldOfView;
        float verticalFOVRadians = verticalFOV * Mathf.Deg2Rad; // Convert degrees to radians

        // Calculate the horizontal field of view using the aspect ratio
        float horizontalFOVRadians = 2 * Mathf.Atan(Mathf.Tan(verticalFOVRadians / 2) * cam.aspect);

        float Left = transform.position.x - Mathf.Tan(horizontalFOVRadians / 2) * -1 * transform.position.z;
        float Right = transform.position.x + Mathf.Tan(horizontalFOVRadians / 2) * -1 * transform.position.z;

        float up = transform.position.y - Mathf.Tan((relativeRotation - (verticalFOV / 2)) * Mathf.Deg2Rad) * -1 * transform.position.z;
        float down = transform.position.y - Mathf.Tan((relativeRotation + verticalFOV / 2) * Mathf.Deg2Rad) * -1 * transform.position.z;

        horizontal = Mathf.Clamp(horizontal, scale * bounds.min.x + 8.0f - Left, (scale * bounds.max.x - 8.0f) - Right);
        //Debug.Log("Left: " + Left + " Right: " + Right + "bound x: " + scale * bounds.max.x + ", " + scale * bounds.min.x);
        vertical = Mathf.Clamp(vertical, scale * bounds.min.y - down, scale * bounds.max.y - up);
        
        transform.Translate(horizontal, vertical, 0 , Space.World);
    }
}
