using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChange : MonoBehaviour
{
    public Sprite select_image, unselect_image;
    Sprite currentSprite;

    private void Start()
    {
        currentSprite = GetComponent<Image>().sprite;
    }

    public void ImageChange()
    {
        GetComponent<Image>().sprite = currentSprite == unselect_image ? select_image : unselect_image;
    }

    public void SetImageSelect()
    {
        GetComponent<Image>().sprite = select_image;
    }

    public void SetImageUnSelect()
    {
        GetComponent<Image>().sprite = unselect_image; 
    }
}
