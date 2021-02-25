using System.Collections.Generic;
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

	private readonly int robotPrefixLength = 3;
	private readonly int robotSuffixLength = 4;

	/// <summary>
	/// Species name formats:
	///		Human: [FirstName] [Initial].
	///		Helicid: [LastName] [Initial]. 
	///		Oshunian: [FirstName] [Title]
	///		Vesta: [Prefix-[Name]
	///		Robot: [Alphabetical prefix]-[Numerical suffix]
	/// </summary>

	private static System.Random random;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			random = new System.Random();
			DontDestroyOnLoad(gameObject);
        }
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	public async void Init()
	{
        List<Task> pilotNameTasks = new List<Task>
        {
            Task.Factory.StartNew(() => HumanMaleNames = LoadNamePoolAsync(PilotsConstants.humanMaleNamesPath).Result),
            Task.Factory.StartNew(() => HumanFemaleNames = LoadNamePoolAsync(PilotsConstants.humanFemaleNamesPath).Result),
            Task.Factory.StartNew(() => HelicidNames = LoadNamePoolAsync(PilotsConstants.helicidNamesPath).Result),
            Task.Factory.StartNew(() => OshunianNames = LoadNamePoolAsync(PilotsConstants.oshunianNamesPath).Result),
            Task.Factory.StartNew(() => OshunianTitles = LoadNamePoolAsync(PilotsConstants.oshunianTitlesPath).Result),
            Task.Factory.StartNew(() => VestaPrefixes = LoadNamePoolAsync(PilotsConstants.vestaPrefixesPath).Result),
            Task.Factory.StartNew(() => VestaNames = LoadNamePoolAsync(PilotsConstants.vestaNamesPath).Result)
        };
		await Task.WhenAll(pilotNameTasks).ContinueWith(x => PilotsManager.Instance.RandomisePilots());
	}

	private async Task<string[]> LoadNamePoolAsync(string fileName)
    {
		string namePool = await DataUtils.ReadListOfTextAsync(fileName);
		return namePool.Split('\n');
    }

	// Combine the value returned with an initial, digit, or second portion
	private string GenerateNamePortion(string[] namePool)
	{
		return namePool[random.Next(0, namePool.Length)];
	}

	private char GenerateInitial()
	{
		return char.ToUpper((char)('a' + random.Next(0, 26)));
	}

	private int GenerateDigit()
	{
		return random.Next(0, 9);
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