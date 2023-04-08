using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnScreen : MonoBehaviour
{
    [SerializeField] private RectTransform uiObject;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease easeType = Ease.OutExpo;
    [SerializeField] private Side side = Side.Left;

    [SerializeField] private bool startMoveOnScreen;
    [SerializeField] private bool startMoveOffScreen;

    private Dictionary<Side, Vector2> offScreenPositions;

    private void Awake()
    {
        offScreenPositions = new Dictionary<Side, Vector2>
        {
            { Side.Right, new Vector2(canvas.pixelRect.width * 2f, uiObject.anchoredPosition.y) },
            { Side.Left, new Vector2(-canvas.pixelRect.width * 2f, uiObject.anchoredPosition.y) }
        };
    }

    public void MoveObjectOnScreen(Side side)
    {
        uiObject.anchoredPosition = offScreenPositions[side];
        uiObject.DOAnchorPos(Vector2.zero, duration).SetEase(easeType);
    }

    public void MoveObjectOffScreen(Side side)
    {
        uiObject.DOAnchorPos(offScreenPositions[side], duration).SetEase(easeType);
    }

    private void Update()
    {
        if (startMoveOnScreen)
        {
            MoveObjectOnScreen(side);
            startMoveOnScreen = false;
        }

        if (startMoveOffScreen)
        {
            MoveObjectOffScreen(side);
            startMoveOffScreen = false;
        }
    }
}