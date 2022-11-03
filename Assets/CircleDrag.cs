using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircleDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static Vector2 defaultPosition;

       public void OnBeginDrag(PointerEventData eventData)//드래그 시작
        {
            defaultPosition = this.transform.position;
        }

       public void OnDrag(PointerEventData eventData)//드래그 중
        {
            Vector2 currentPos = Input.mousePosition;
            this.transform.position = currentPos;
        }

       public void OnEndDrag(PointerEventData eventData)//드래그 끝
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = defaultPosition;
        }

}
