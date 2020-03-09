using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectONOFF : MonoBehaviour
{
    [SerializeField] private GameObject obj;


    public void ActivateObject(bool isActive)
    {
        obj.SetActive(isActive);
    }


}
