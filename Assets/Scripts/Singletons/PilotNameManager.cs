using System;
using System.Threading.Tasks;
using UnityEngine;

public class PilotNameManager : MonoBehaviour
{
	public static PilotNameManager Instance { get; private set; }
	public string[] HumanMaleNames { get; private set; }
	public string[] HumanFemaleNames { get; private set; }
	public string[] HelicidNames { get; private set; }
	public string[] OshunianNames { get; private set; }
	public string[] OshunianTitles { get; private set; }
	public string[] VestaPrefixes { get; private set; }
	public string[] VestaNames { get; private set; }

	public static event Action OnPilotNamesLoaded;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
        }
		else
		{
			Destroy(gameObject);
		}
	}

	public void Init()
    {
		LoadNamePoolsAsync();
	}

	private async void LoadNamePoolsAsync()
	{
		HumanMaleNames = await LoadNamePoolAsync(PilotsConstants.humanMaleNamesPath);
		HumanFemaleNames = await LoadNamePoolAsync(PilotsConstants.humanFemaleNamesPath);
		HelicidNames = await LoadNamePoolAsync(PilotsConstants.helicidNamesPath);
		OshunianNames = await LoadNamePoolAsync(PilotsConstants.oshunianNamesPath);
		OshunianTitles = await LoadNamePoolAsync(PilotsConstants.oshunianTitlesPath);
		VestaPrefixes = await LoadNamePoolAsync(PilotsConstants.vestaPrefixesPath);
		VestaNames = await LoadNamePoolAsync(PilotsConstants.vestaNamesPath);
		OnPilotNamesLoaded?.Invoke();
	}

	private async Task<string[]> LoadNamePoolAsync(string fileName)
    {
		string namePool = await DataUtils.ReadListOfTextAsync(fileName);
		return namePool.Split('\n');
    }

	/// <summary>
	/// Species name formats:
	///		Human: [FirstName] [Initial].
	///		Helicid: [LastName] [Initial]. 
	///		Oshunian: [FirstName] [Title]
	///		Vesta: [Prefix-[Name]
	///		Robot: [Alphabetical prefix]-[Numerical suffix]
	/// </summary>

	private readonly int robotPrefixLength = 3;
	private readonly int robotSuffixLength = 4;

	// Combine the value returned with an initial, digit, or second portion
	private string GenerateNamePortion(string[] namePool)
	{
		return namePool[UnityEngine.Random.Range(0, namePool.Length)];
	}

	private char GenerateInitial()
	{
		return char.ToUpper((char)('a' + UnityEngine.Random.Range(0, 26)));
	}

	private int GenerateDigit()
	{
		return UnityEngine.Random.Range(0, 9);
	}

	public string GetRandomName(Species species)
	{
		switch (species)
		{
			case Species.HumanMale:
				var maleFirstName = GenerateNamePortion(Instance.HumanMaleNames);
				var maleSurname = GenerateInitial();
				return $"{maleFirstName} {maleSurname}.";

			case Species.HumanFemale:
				var femaleFirstName = GenerateNamePortion(Instance.HumanFemaleNames);
				var femaleSurname = GenerateInitial();
				return $"{femaleFirstName} {femaleSurname}.";

			case Species.Helicid:
				var helicidSurname = GenerateNamePortion(Instance.HelicidNames);
				var helicidFirstName = GenerateInitial();

				// Helicid surnames come first 
				return $"{helicidSurname} {helicidFirstName}.";

			case Species.Oshunian:
				var oshunianFirstName = GenerateNamePortion(Instance.OshunianNames);
				var oshunianSurname = GenerateNamePortion(Instance.OshunianTitles);

				// No space required since the surname is a title with a space built in
				return $"{oshunianFirstName}{oshunianSurname}";

			case Species.Vesta:
				var vestaPefix = GenerateNamePortion(Instance.VestaPrefixes);
				var vestaName = GenerateNamePortion(Instance.VestaNames);
				return $"{vestaPefix}-{vestaName}";

			case Species.Robot:
				string prefix = string.Empty;
				string suffix = string.Empty;

				for (int i = 0; i < robotPrefixLength; i++)
				{
					prefix += GenerateInitial();
				}
				for (int i = 0; i < robotSuffixLength; i++)
				{
					suffix += GenerateDigit().ToString();
				}

				return $"{prefix}-{suffix}";

			default:
				return string.Empty;
		}
	}
}