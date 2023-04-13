using UnityEngine;

public class InteractiveCardCycle : ICyclable<GameObject>
{
    public GameObject[] CyclableContent { get; set; }

    private int currentIndex;
    private bool OnLastCard => currentIndex >= CyclableContent.Length - 1;
    private GameObject CurrentContent => CyclableContent[currentIndex];

    public void Cycle()
    {

    }
}