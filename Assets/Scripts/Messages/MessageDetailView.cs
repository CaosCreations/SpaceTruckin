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
        messageDetail.text += "Subject: " + message.subject + "\n";
        messageDetail.text += message.body;
    }

    public void SetJobAcceptButton(Job job)
    {
        jobAcceptButton.GetComponentInChildren<Text>().text = job.title; 
        jobAcceptButton.onClick.RemoveAllListeners();
        jobAcceptButton.onClick.AddListener(() => onJobAccept?.Invoke(job)); 
    }

    // Disable if the message doesn't include a job offer 
    public void DisableJobAcceptButton()
    {
        jobAcceptButton.gameObject.SetActive(false);
    }
}