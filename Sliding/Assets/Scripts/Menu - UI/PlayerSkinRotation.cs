using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkinRotation : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Transform SkinsPivot;

    Vector3 rotation;

    public void OnDrag(PointerEventData eventData)
    {
        rotation.y -= eventData.delta.x;
        SkinsPivot.transform.localRotation = Quaternion.Euler(rotation);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Hi");
    }

    private void OnMouseEnter()
    {
        Debug.Log("DIO MERDA");
    }
}
