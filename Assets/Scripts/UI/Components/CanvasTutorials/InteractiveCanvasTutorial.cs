﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class InteractiveCanvasTutorial : SubMenu
{
    [SerializeField] protected InteractiveCanvasTutorialCard openingCard;
    [SerializeField] protected InteractiveCanvasTutorialCard endingCard;
    [SerializeField] protected bool lockCanvas;
    [SerializeField] protected InteractiveCanvasTutorialCard cantExitCard;
    [SerializeField] protected string dialogueBoolOnComplete;
    [SerializeField] protected Cutscene cutsceneOnComplete;
    [SerializeField] protected int conversationIdOnComplete;

    [field: SerializeField]
    public bool DoNotAutomaticallyOpen { get; private set; }

    protected Stack<InteractiveCanvasTutorialCard> seenCards = new();
    private UnityAction onComplete;

    public void Init(UnityAction onComplete = null)
    {
        this.onComplete = onComplete;
    }

    protected override void OnEnable()
    {
        if (lockCanvas)
        {
            base.OnEnable();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (endingCard != null && endingCard.gameObject.activeSelf)
        {
            EndTutorial();
        }
    }

    protected virtual void Start()
    {
        ShowCard(openingCard);

        // Optional as some tutorials end via listening for external events or a combination of shown flags
        if (endingCard != null)
        {
            endingCard.OnClosed += EndingCardHandler;
        }

        if (lockCanvas)
        {
            LockCanvas();
        }
    }

    protected void LockCanvas()
    {
        UIManager.UniversalUI.DisableCloseWindowButton();

        if (cantExitCard != null)
        {
            UIManager.UniversalUI.AddCloseWindowButtonListener(CloseWindowButtonHandler);
        }
        else
        {
            Debug.LogWarning("\"Can't exit\" card for locking canvas does not exist");
        }
    }

    protected void UnlockCanvas()
    {
        UIManager.UniversalUI.RemoveCloseWindowButtonListener(CloseWindowButtonHandler);
        UIManager.UniversalUI.EnableCloseWindowButton();
        RemoveOverriddenKeys();
        lockCanvas = false;
    }

    protected virtual void EndingCardHandler()
    {
        EndTutorial();
    }

    protected virtual void EndTutorial()
    {
        if (lockCanvas)
        {
            UnlockCanvas();
        }

        if (!string.IsNullOrWhiteSpace(dialogueBoolOnComplete))
        {
            DialogueDatabaseManager.Instance.UpdateDatabaseVariable(dialogueBoolOnComplete, true);
        }
        if (cutsceneOnComplete != null)
        {
            TimelineManager.PlayCutscene(cutsceneOnComplete);
        }
        if (conversationIdOnComplete > 0)
        {
            DialogueUtils.StartConversationById(conversationIdOnComplete);
        }
        onComplete?.Invoke();
        RemoveOverriddenKeys();
        Destroy(gameObject);
    }

    protected void CloseWindowButtonHandler()
    {
        ShowCard(cantExitCard, false);
    }

    protected abstract void CloseAllCards();

    protected virtual void ShowCard(InteractiveCanvasTutorialCard card, bool closeOtherCards = true)
    {
        if (card != cantExitCard)
        {
            seenCards.Push(card);
        }

        if (closeOtherCards)
        {
            CloseAllCards();
        }

        card.SetActive(true);
        if (card.UnlockCanvas)
        {
            UnlockCanvas();
        }
        card.SetSpotlightOjectsActive(true);
    }

    protected virtual void ToPreviousCard()
    {
        seenCards.Pop();
        ShowCard(seenCards.Peek());
    }

    protected virtual void ShowCard(InteractiveCanvasTutorialCard card, ref bool cardShown, Button button, UnityAction buttonHandler)
    {
        if (cardShown)
            return;

        ShowCard(card);
        cardShown = true;
        button.onClick.RemoveListener(buttonHandler);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.ExitKey) && lockCanvas && cantExitCard != null)
        {
            CloseWindowButtonHandler();
        }
    }
}