using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenMoveSide
{
    Left, Right
}

public class MoveOnScreen : MonoBehaviour
{
    [SerializeField] private RectTransform uiObject;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Vector2 centeredPercentage = new(0.5f, 0.5f);
    [SerializeField] private Ease easeType = Ease.OutExpo;
    [SerializeField] private ScreenMoveSide side = ScreenMoveSide.Left;

    [SerializeField] private bool startMoveOnScreen;
    [SerializeField] private bool startMoveOffScreen;

    private Dictionary<ScreenMoveSide, Vector2> offScreenPositions;
    private Dictionary<ScreenMoveSide, Vector2> onScreenPositions;

    private void Awake()
    {
        offScreenPositions = new Dictionary<ScreenMoveSide, Vector2>
        {
            { ScreenMoveSide.Right, new Vector2(canvas.pixelRect.width * 2f, uiObject.anchoredPosition.y) },
            { ScreenMoveSide.Left, new Vector2(-canvas.pixelRect.width * 2f, uiObject.anchoredPosition.y) }
        };

        onScreenPositions = new Dictionary<ScreenMoveSide, Vector2>
        {
            { ScreenMoveSide.Right, new Vector2(canvas.pixelRect.width * centeredPercentage.x, canvas.pixelRect.height * centeredPercentage.y) },
            { ScreenMoveSide.Left, new Vector2(-(canvas.pixelRect.width * centeredPercentage.x), canvas.pixelRect.height * centeredPercentage.y) }
        };

        startMoveOnScreen = false;
        startMoveOffScreen = false;
    }

    public void MoveObjectOnScreen(ScreenMoveSide side)
    {
        uiObject.anchoredPosition = offScreenPositions[side];
        uiObject.DOAnchorPos(Vector2.zero, duration).SetEase(easeType);
        //uiObject.DOAnchorPos(onScreenPositions[side], duration).SetEase(easeType
    }

    public void MoveObjectOffScreen(ScreenMoveSide side)
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