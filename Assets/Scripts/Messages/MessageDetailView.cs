using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDetailView : MonoBehaviour
{
    public Text messageDetail;
    public Button jobAcceptButton;

    public static event Action<Job> onJobAccept;

    public void SetMessage(Message message)
    {
        messageDetail.text = "From: " + message.sender + "\n";
        messageDetail.text += "Subject: " + message.subject + "\n\n";
        messageDetail.text += message.body + "\n";

        if (message.job != null)
        {
            messageDetail.text += "I've got a job for you.";
        }
    }

    public void SetJobAcceptButton(Job job)
    {
        Text buttonText = jobAcceptButton.GetComponentInChildren<Text>();
        buttonText.text = "Accept " + job.title;

        jobAcceptButton.interactable = job.isAccepted ? false : true;  

        jobAcceptButton.onClick.RemoveAllListeners();
        jobAcceptButton.onClick.AddListener(() =>
        {
            buttonText.text = "Job Accepted!";
            jobAcceptButton.interactable = false;
            onJobAccept?.Invoke(job);
        });
    }
}