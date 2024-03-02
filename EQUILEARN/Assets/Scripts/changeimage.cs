using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Sprite initialButtonImage;
    public Sprite newButtonImage;
    public Button button;
    private bool isInitialImage = true;

    // Start is called before the first frame update
    void Start()
    {
        button.image.sprite = initialButtonImage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeButtonImage()
    {
        if (isInitialImage)
        {
            button.image.sprite = newButtonImage;
        }
        else
        {
            button.image.sprite = initialButtonImage;
        }

        isInitialImage = !isInitialImage;
    }
}

