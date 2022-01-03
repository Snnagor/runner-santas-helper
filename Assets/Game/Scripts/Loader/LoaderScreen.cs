using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoaderScreen : MonoBehaviour
{
    [Header("Vertical")]
    [SerializeField] private Sprite vertScreenImage;
    [SerializeField] private Sprite vertHeaderImage;
    [SerializeField] private Sprite vertElfImage;
    [Space]
    [SerializeField] private float widthElfVer;
    [SerializeField] private float heightElfVer;
    [Space]
    [SerializeField] private float widthHeaderVer;
    [SerializeField] private float heightHeaderVer;
    [Header("Horizontal")]
    [SerializeField] private Sprite horScreenImage;
    [SerializeField] private Sprite horHeaderImage;
    [SerializeField] private Sprite horElfImage;
    [Space]
    [SerializeField] private float widthElfHor;
    [SerializeField] private float heightElfHor;
    [Space]
    [SerializeField] private Image screenPanel;
    [SerializeField] private Image headerImage;
    [SerializeField] private Image elfImage;
    [SerializeField] private RectTransform buttonPlay;
    
    private bool isVer;

    private void Start()
    {
        ChangeScreen();
    }

    private void ChangeScreen()
    {
        if(Screen.height > Screen.width)
        {
            screenPanel.sprite = vertScreenImage;
            headerImage.sprite = vertHeaderImage;
            elfImage.sprite = vertElfImage;

            buttonPlay.localPosition = new Vector2(buttonPlay.localPosition.x, -458f);
            buttonPlay.sizeDelta = new Vector2(180f, 180f);

            screenPanel.SetNativeSize();            

            isVer = true;
        }
        else
        {
            screenPanel.sprite = horScreenImage;
            headerImage.sprite = horHeaderImage;
            elfImage.sprite = horElfImage;

            buttonPlay.localPosition = new Vector2(buttonPlay.localPosition.x, -388f);
            buttonPlay.sizeDelta = new Vector2(200f, 200f);

            headerImage.SetNativeSize();
            elfImage.SetNativeSize();
            screenPanel.SetNativeSize();

            RectTransform headerRect = headerImage.GetComponent<RectTransform>();
            headerRect.localPosition = new Vector2(headerRect.localPosition.x, headerRect.localPosition.y + 62f); 

            isVer = false;
        }
    }

    private void Update()
    {
        if(Screen.height > Screen.width && !isVer)
        {
            ChangeScreen();
            headerImage.SetNativeSize();
            elfImage.SetNativeSize();

            elfImage.GetComponent<RectTransform>().sizeDelta = new Vector2(widthElfVer, heightElfVer);

            RectTransform headerRect = headerImage.GetComponent<RectTransform>();

            headerRect.sizeDelta = new Vector2(widthHeaderVer, heightHeaderVer);
            headerRect.localPosition = new Vector2(headerRect.localPosition.x, headerRect.localPosition.y - 62f);
        }

        if (Screen.height < Screen.width && isVer)
        {
            ChangeScreen();
        }
    }

    
}
