using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            Touch touch = Input.GetTouch(0);

            RaycastHit hit;


            Vector3 tapPosition = camera.ScreenToWorldPoint(touch.position);
            var ray = camera.ScreenPointToRay(touch.position);

            if(Physics.Raycast(ray, out hit, 0, 50, QueryTriggerInteraction.Ignore))
            {

                if(hit.transform.gameObject.GetComponent<IOnSelected>() != null)
                {
                      hit.transform.gameObject.GetComponent<IOnSelected>().HandleOnSelected();
                    Debug.DrawRay(tapPosition, (hit.transform.position - this.transform.position), Color.yellow, 1f);
                    Debug.Log("Did Hit");
                }
              

            }
            else
            {
                Debug.DrawRay(tapPosition, transform.TransformDirection(Vector3.forward), Color.white, 1f);
                Debug.Log("Did not Hit");
            }
       
          
        }
#endif

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            // Bit shift the index of the layer (8) to get a bit mask
            // int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            //layerMask = ~layerMask;

            Vector3 tapPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

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
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 26);
            Vector3 curPosition = camera.ScreenToWorldPoint(mousePosition);
            currentObject.transform.position = curPosition;

        }


        if (Input.GetMouseButtonUp(0))
        {
            if(selectable != null)
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


}
