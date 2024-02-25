using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ScrollerDragManager : MonoBehaviour , IBeginDragHandler ,IEndDragHandler , IPointerDownHandler
{
    public ScrollRect scrollRect;
    [SerializeField] public List<ScrollRange> scrollRangeForPages = new List<ScrollRange>();

    float beginTime;
    float swipeStartPoint;
    public void OnBeginDrag(PointerEventData eventData)
    {
        beginTime = Time.time;
        swipeStartPoint = Input.mousePosition.x;
       // scrollRect.content.transform.DOKill();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.enabled = false;
        if (Time.time <= beginTime + 0.25f)
        {
            if(Input.mousePosition.x> swipeStartPoint)
            {
                ScrollingPage.instance.MoveTo(eDirection.right);
            
            }
            else
            {
                ScrollingPage.instance.MoveTo(eDirection.left);
               
            }
           
            return;
        }
      
        float currentPos = scrollRect.normalizedPosition.x;
       
        for (int i = 0; i < scrollRangeForPages.Count; i++)
        {
            if (currentPos >= scrollRangeForPages[i].range.x && currentPos < scrollRangeForPages[i].range.y)
            {
                scrollRect.content.transform.DOLocalMoveX(scrollRangeForPages[i].val,0.5f).OnComplete(()=> {

                    ScrollingPage.instance.UpdateButtons((int)(Mathf.Abs(( scrollRangeForPages[i].val / 1080f))));
                });
               
                //scrollRect.normalizedPosition = new Vector2(scrollRangeForPages[i].val, 0);
                return;
            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        scrollRect.enabled = true;
    }

    
}
public enum eDirection
{
    none,right,left
}