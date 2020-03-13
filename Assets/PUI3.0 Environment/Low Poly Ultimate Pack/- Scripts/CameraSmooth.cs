using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour {

    bool isStarted = false;
    public float movingForce = 5f;

    public float zoomAngle = 11;
    public float zoomHeight = 8;
    public float zoomSpeed = 2;

    Vector3 defaultRotation;
    Vector3 defaultPosition;
   
    bool zoomingOut = false;
    bool zoomingIn = false;

    float interval = 0.05f;

    float intervalCache;
    Vector3 mouseForce;
    Vector3 LastMousePosition;

    // Use this for initialization
    void Start () {
        defaultPosition = transform.position;
        defaultRotation = transform.localEulerAngles;

        intervalCache = interval;

    }

    // Update is called once per frame
    void Update () {
		if(Input.GetMouseButton(0))
        {

            if(!isStarted) {
                StartMove();
            }
            Move();
        }

        if(Input.GetMouseButtonUp(0))
        {
            StopMove();
        }

        float scrollNum = Input.GetAxis("Mouse ScrollWheel");
        if (scrollNum < 0f && !zoomingOut)
        {
            StopAllCoroutines();
            StartCoroutine(ZoomOut());
        }
        else if (scrollNum > 0f && !zoomingIn)
        {
            StopAllCoroutines();
            StartCoroutine(ZoomIn());
        }
    }

    void StartMove()
    {
        LastMousePosition = Input.mousePosition;
        mouseForce = new Vector3(0, 0, 0);
        isStarted = true;
       
    }

    void StopMove()
    {
        mouseForce = new Vector3(0, 0, 0);
        LastMousePosition = Vector3.zero;
        isStarted = false;
    }


    void Move()
    {
       
        if(interval>0)
        {
            mouseForce.x = (Input.mousePosition.x - LastMousePosition.x) / Screen.width / Time.deltaTime * movingForce;
            mouseForce.y = transform.position.y;
            mouseForce.z = (Input.mousePosition.y - LastMousePosition.y) / Screen.height / Time.deltaTime * movingForce;
            LastMousePosition = Input.mousePosition;

        }
        else if(interval == intervalCache)
        {
            StopAllCoroutines();
            StartCoroutine(SetCameraPosition(mouseForce));
            mouseForce = new Vector3(0, 0, 0);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(SetCameraPosition(mouseForce));
            mouseForce = new Vector3(0, 0, 0);
            interval = intervalCache;
        }

        interval -= Time.deltaTime;
    }
    IEnumerator SetCameraPosition(Vector3 mouseForce)
    {
        Vector3 destination = new Vector3(transform.position.x + -mouseForce.x, transform.position.y, transform.position.z + -mouseForce.z);

        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {

            transform.position = Vector3.Lerp(transform.position, destination, 2 * Time.deltaTime);

            yield return null;

        }

        yield return null;

    }


    IEnumerator ZoomIn()
    {
       
        zoomingOut = false;
        zoomingIn = true;
        Vector3 zoomedPosition = new Vector3(transform.position.x, zoomHeight, transform.position.z);
        Vector3 zoomedRotation = new Vector3(zoomAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);

        while (Vector3.Distance(transform.position, zoomedPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, zoomedPosition , zoomSpeed * Time.deltaTime);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, zoomedRotation, zoomSpeed * Time.deltaTime);

            yield return null;
            

        }
        yield return null;

    }


    IEnumerator ZoomOut()
    {

        zoomingIn = false;
        zoomingOut = true;
        Vector3 zoomedPosition = new Vector3(transform.position.x, defaultPosition.y, transform.position.z);
        Vector3 zoomedRotation = new Vector3(defaultRotation.x, transform.localEulerAngles.y, transform.localEulerAngles.z);

        while (Vector3.Distance(transform.position, zoomedPosition) > 0.1f)
        {


            transform.position = Vector3.Lerp(transform.position, zoomedPosition, zoomSpeed * Time.deltaTime);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, zoomedRotation, zoomSpeed * Time.deltaTime);

            yield return null;
            
        }
        yield return null;
    }

 

 

}   
