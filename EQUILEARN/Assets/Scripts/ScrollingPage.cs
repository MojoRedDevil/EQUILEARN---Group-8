using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollingPage : MonoBehaviour
{
    public static ScrollingPage instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public ScrollRect scrollRect;
    void Start()
    {
        scrollRect.normalizedPosition = new Vector2(0.5f, 0);
        UpdateButtons(2);
    }
    public List<Button> listOfButtons;
    public Sprite spriteGreenBtn;
    public Sprite spriteGrayBtn;
    int currentId;
    public void UpdateButtons(int id)
    {
        for(int i = 0; i < listOfButtons.Count; i++)
        {
            listOfButtons[i].image.sprite = spriteGrayBtn;
        }
        listOfButtons[id].image.sprite = spriteGreenBtn;
        currentId = id;
    }
    public void ButtonClick(int id)
    {
        scrollRect.content.transform.DOLocalMoveX(-id*1080, 0.5f);
        UpdateButtons(id);
    }
    public void MoveTo(eDirection direction)
    {
        switch (direction)
        {
            case eDirection.left:
                if (currentId < listOfButtons.Count - 1)
                {
                    currentId++;
                    ButtonClick(currentId);
                }
                else
                {
                    ButtonClick(currentId);
                }
                break;
            case eDirection.right:
                if (currentId >0)
                {
                    currentId--;
                    ButtonClick(currentId);
                }
                else
                {
                    ButtonClick(currentId);
                }
                break;
        }
       
    }
    public void StopScrollerAt(float val)
    {
        scrollRect.normalizedPosition =new  Vector2(val, 0);
    }
  
}
[System.Serializable]
public class ScrollRange
{
    public float val;
    public Vector2 range;
}