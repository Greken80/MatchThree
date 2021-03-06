﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractionScreenTap : MonoBehaviour
{

    [SerializeField] private Camera camera;


    private GameObject currentObject;

    private ISelectable selectable;

    


    private void Awake()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

    }

    private void Update()
    {
  
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsPointerOverUIObject())
                {
                    return;
                }
                Vector3 tapPosition = camera.ScreenToWorldPoint(touch.position);
                var ray = camera.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
             
                    MonoBehaviour[] list = hit.transform.gameObject.GetComponentsInChildren<MonoBehaviour>();

                    selectable = CheckIfObjectHasInterface(list);


                    if (selectable == null)
                    {
                        return;
                    }
#if UNITY_EDITOR
                    Debug.DrawRay(tapPosition, (hit.transform.position - this.transform.position) * 100, Color.yellow, 1f);
                    Debug.Log("Did Hit");
#endif

                    currentObject = hit.transform.gameObject;
                    selectable.OnSelected();
                }
                else
                {
#if UNITY_EDITOR
                    Debug.DrawRay(tapPosition, transform.TransformDirection(Vector3.forward), Color.white, 1f);
                    Debug.Log("Did not Hit");
#endif
                }      
            }

            if (touch.phase == TouchPhase.Moved && currentObject != null)
            {
                Vector3 curPosition = camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y,  -camera.transform.position.z));

                currentObject.transform.position = curPosition;

            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (selectable != null)
                {
                    selectable.OnDeselected();
                    currentObject = null;
                    selectable = null;

                }

            }

        }


#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 tapPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(BoardManager.Instance.IsShifting)
            {
                return;
            }

            if (Physics.Raycast(ray, out hit))
            {

                MonoBehaviour[] list = hit.transform.gameObject.GetComponentsInChildren<MonoBehaviour>();

                selectable = CheckIfObjectHasInterface(list);

                if (selectable == null)
                {
                    return;
                }

                Debug.DrawRay(tapPosition, (hit.transform.position - this.transform.position) * 100, Color.yellow, 1f);
                Debug.Log("Did Hit");

                currentObject = hit.transform.gameObject;

                selectable.OnSelected();

            }
            else
            {
                Debug.DrawRay(tapPosition, transform.TransformDirection(ray.origin - camera.transform.position) * 100, Color.magenta, 1f, false);
                Debug.Log("Did not Hit");
            }
        }

        if (Input.GetMouseButton(0) && currentObject != null)
        {
            Vector3 curPosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,Input.mousePosition.z - camera.transform.position.z));
 
            currentObject.transform.position = curPosition;
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (selectable != null)
            {
                selectable.OnDeselected();
                currentObject = null;
                selectable = null;

            }
        }
#endif
    }

    ISelectable CheckIfObjectHasInterface(MonoBehaviour[] list)
    {
        foreach (MonoBehaviour mb in list)
        {
            if (mb is ISelectable)
            {
                return (ISelectable)mb;
            }
        }
        return null;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }



}
