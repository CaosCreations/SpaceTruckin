using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PilotNameDataSingleton : MonoBehaviour
{
    public static PilotNameDataSingleton Instance { get; private set; }
    public string[] HumanMaleNames { get; private set; }
    public string[] HumanFemaleNames { get; private set; }
    public string[] HelicidNames { get; private set; }
    public string[] OshunianNames { get; private set; }
    public string[] OshunianTitles { get; set; }
    public string[] VestaPrefixes { get; set; }
    public string[] VestaNames { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            HumanMaleNames = File.ReadAllLines(PilotsConstants.humanMaleNamesPath); 
            HumanFemaleNames = File.ReadAllLines(PilotsConstants.humanFemaleNamesPath);
            HelicidNames = File.ReadAllLines(PilotsConstants.helicidNamesPath);
            OshunianNames = File.ReadAllLines(PilotsConstants.oshunianNamesPath);
            OshunianTitles = File.ReadAllLines(PilotsConstants.oshunianTitlesPath);
            VestaPrefixes = File.ReadAllLines(PilotsConstants.vestaPrefixesPath);
            VestaNames = File.ReadAllLines(PilotsConstants.vestaNamesPath);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
