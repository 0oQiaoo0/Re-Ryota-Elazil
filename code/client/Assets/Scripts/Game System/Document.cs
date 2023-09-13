using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Document : MonoBehaviour ,IPointerDownHandler,IPointerMoveHandler,IPointerExitHandler
{
    public GameController Controller;
    public bool isDragging;
    public bool isGhost;
    public Vector2 initialPosition;
    [SerializeField]private List<Vector2> positionList = new List<Vector2>();

    private int positionListSize;
    private int positionListIndex;

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private void Awake()
    {
        isDragging = false;
        isGhost = false;
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        positionList.Clear();
        positionList.Add(initialPosition);
        positionListSize = 1;
        positionListIndex = 0;
    }
    public void DocumentReset()
    {
        gameObject.SetActive(true);
        isDragging = false;
        isGhost = false;
        positionList.Clear();
        positionList.Add(initialPosition);
        positionListSize = 1;
        positionListIndex = 0;
        rectTransform.anchoredPosition = initialPosition;

        Color tmp = GetComponent<Image>().color;
        tmp.a = 1f;
        GetComponent<Image>().color = tmp;
    }

    public void GhostReset()
    {
        positionListIndex = positionListSize - 1;
        rectTransform.anchoredPosition = positionList[positionListIndex];
        gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isGhost && !Controller.isDragging) 
        {
            isDragging = true;
            Controller.isDragging = true;
            Controller.StartGhost();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            Controller.isDragging = false;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(isDragging)
        {
            Vector2 pos = UI.UIRenderGraphicRaycasterProxy.ConvertPosition( eventData.position);
            // Vector2 pos = eventData.position;
            Vector2 uiPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, pos,
                eventData.enterEventCamera, out uiPos))
                rectTransform.localPosition = uiPos;
            
            // GetComponent<RectTransform>().anchoredPosition += eventData.delta;
        }
    }

    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collising with" + collision.name);
        if(isDragging)
        {
            if (collision.gameObject.CompareTag("Folder"))//�����յ��ļ���
            {
                if (collision.GetComponent<Folder>().isEmpty)
                {
                    isDragging = false;
                    isGhost = true;

                    positionListSize = positionList.Count;
                    positionListIndex = positionListSize - 1;

                    Controller.isDragging = false;
                    Controller.ResetGhost();

                    collision.GetComponent<Folder>().FloderClose();

                    Color tmp = GetComponent<Image>().color;
                    tmp.a = 100f / 255f;
                    GetComponent<Image>().color = tmp;

                    gameObject.SetActive(false);
                }
            }
            else if (collision.gameObject.CompareTag("Document"))
            {
                if (collision.GetComponent<Document>().isGhost)
                {
                    Controller.GameReset();
                }
            }
            else
            {
                Controller.GameReset();
            }
        }
    }
    private void FixedUpdate()
    {
        if (isGhost)
        {
            if(positionListIndex > 0)
            {
                GetComponent<RectTransform>().anchoredPosition = positionList[--positionListIndex];
            }
        }
        else if (isDragging)
        {
            positionList.Add(GetComponent<RectTransform>().anchoredPosition);
        }
    }
}
