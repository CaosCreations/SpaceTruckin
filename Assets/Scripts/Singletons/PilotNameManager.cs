using System.Threading.Tasks;
using UnityEngine;

public class PilotNameManager : MonoBehaviour
{
	public static PilotNameManager Instance { get; private set; }
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
			LoadNamePoolsAsync();
        }
		else
		{
			Destroy(gameObject);
		}
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
	}

	private async Task<string[]> LoadNamePoolAsync(string fileName)
    {
		string namePool = await DataUtils.ReadTextFileAsync(fileName);
		return namePool.Split('\n');
    }

	/// <summary>
	/// Name formats:
	///		Human: [FirstName] [Initial].
	///		Helicid: [LastName] [Initial]. 
	///		Oshunian: [FirstName] [Title]
	///		Vesta: [Prefix-[Name]
	///		Robot: [Alphabetical prefix]-[Numerical suffix]
	/// </summary>

	private enum NameCategory
	{
		HumanMale, HumanFemale, Helicid, Oshunian, Vesta, Robot
	};

	private readonly int robotPrefixLength = 3;
	private readonly int robotSuffixLength = 4;

	// Combine the value returned with an initial, digit, or second portion
	private string GenerateNamePortion(string[] namePool)
	{
		return namePool[Random.Range(0, namePool.Length)];
	}

	private char GenerateInitial()
	{
		return char.ToUpper((char)('a' + Random.Range(0, 26)));
	}

	private int GenerateDigit()
	{
		return Random.Range(0, 9);
	}

	private string GetRandomName(NameCategory nameCategory)
	{
		switch (nameCategory)
		{
			case NameCategory.HumanMale:
				var maleFirstName = GenerateNamePortion(Instance.HumanMaleNames);
				var maleSurname = GenerateInitial();
				return $"{maleFirstName} {maleSurname}.";

			case NameCategory.HumanFemale:
				var femaleFirstName = GenerateNamePortion(Instance.HumanFemaleNames);
				var femaleSurname = GenerateInitial();
				return $"{femaleFirstName} {femaleSurname}.";

			case NameCategory.Helicid:
				var helicidSurname = GenerateNamePortion(Instance.HelicidNames);
				var helicidFirstName = GenerateInitial();

				// Helicid surnames come first 
				return $"{helicidSurname} {helicidFirstName}.";

			case NameCategory.Oshunian:
				var oshunianFirstName = GenerateNamePortion(Instance.OshunianNames);
				var oshunianSurname = GenerateNamePortion(Instance.OshunianTitles);

				// No space required since the surname is a title with a space built in
				return $"{oshunianFirstName}{oshunianSurname}";

			case NameCategory.Vesta:
				var vestaPefix = GenerateNamePortion(Instance.VestaPrefixes);
				var vestaName = GenerateNamePortion(Instance.VestaNames);
				return $"{vestaPefix}-{vestaName}";

			case NameCategory.Robot:
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