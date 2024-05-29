using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInteraction : MonoBehaviour
{
    [SerializeField] MapHandler mapHandler;
    [SerializeField] string objectName;


    void OnMouseEnter() {
        mapHandler.OnMouseEnterCollider(objectName);
    }

    void OnMouseExit() {
        mapHandler.OnMouseExitCollider();
    }

    void OnMouseDown() {
        mapHandler.OnMouseClickCollider(objectName);
    }
}
