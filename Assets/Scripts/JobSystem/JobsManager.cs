using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobsManager : MonoBehaviour
{ 

    void Start()
    {
        MessageDetailView.onJobAccept += AcceptJob;
    }

    public void AcceptJob(Job job)
    {
        job.isAccepted = true; 
    }

    void Update()
    {
        
    }
}
