using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PilotAssetsManager : MonoBehaviour
{
	public static PilotAssetsManager Instance { get; private set; }

	// Pilot names 
	public string[] HumanMaleNames { get; private set; }
	public string[] HumanFemaleNames { get; private set; }
	public string[] HelicidNames { get; private set; }
	public string[] OshunianNames { get; private set; }
	public string[] OshunianTitles { get; private set; }
	public string[] VestaPrefixes { get; private set; }
	public string[] VestaNames { get; private set; }

	/// <summary>
	/// Species name formats:
	///		Human: [FirstName] [Initial].
	///		Helicid: [LastName] [Initial]. 
	///		Oshunian: [FirstName] [Title]
	///		Vesta: [Prefix-[Name]
	///		Robot: [Alphabetical prefix]-[Numerical suffix]
	/// </summary>
	/// 

	private readonly int robotPrefixLength = 3;
	private readonly int robotSuffixLength = 4;
	private static System.Random random;

	// Pilot likes and dislikes 
	public static string[] Likes { get; private set; }
	public static string[] Dislikes { get; private set; }

	// Pilot sprite avatars 
	[SerializeField] private Sprite[] humanMaleSprites;
	[SerializeField] private Sprite[] humanFemaleSprites;
	[SerializeField] private Sprite[] vestaSprites;
	[SerializeField] private Sprite[] helicidSprites;
	[SerializeField] private Sprite[] robotSprites;

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
			Task.Factory.StartNew(() => HumanMaleNames = LoadTextPoolAsync(PilotsConstants.humanMaleNamesPath).Result),
			Task.Factory.StartNew(() => HumanFemaleNames = LoadTextPoolAsync(PilotsConstants.humanFemaleNamesPath).Result),
			Task.Factory.StartNew(() => HelicidNames = LoadTextPoolAsync(PilotsConstants.helicidNamesPath).Result),
			Task.Factory.StartNew(() => OshunianNames = LoadTextPoolAsync(PilotsConstants.oshunianNamesPath).Result),
			Task.Factory.StartNew(() => OshunianTitles = LoadTextPoolAsync(PilotsConstants.oshunianTitlesPath).Result),
			Task.Factory.StartNew(() => VestaPrefixes = LoadTextPoolAsync(PilotsConstants.vestaPrefixesPath).Result),
			Task.Factory.StartNew(() => VestaNames = LoadTextPoolAsync(PilotsConstants.vestaNamesPath).Result),
			Task.Factory.StartNew(() => Likes = LoadTextPoolAsync(PilotsConstants.pilotLikesPath).Result),
			Task.Factory.StartNew(() => Dislikes = LoadTextPoolAsync(PilotsConstants.pilotDislikesPath).Result)
		};
		await Task.WhenAll(pilotNameTasks).ContinueWith(x => PilotsManager.Instance.RandomisePilots());
	}

	private async Task<string[]> LoadTextPoolAsync(string fileName)
    {
		string textPool = await DataUtils.ReadFileAsync(fileName);
		return textPool.Split('\n');
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
				var robotPrefix = string.Empty;
				var robotSuffix = string.Empty;

				for (int i = 0; i < robotPrefixLength; i++)
				{
					robotPrefix += GenerateInitial();
				}
				for (int i = 0; i < robotSuffixLength; i++)
				{
					robotSuffix += GenerateDigit().ToString();
				}

				return $"{robotPrefix}-{robotSuffix}";

			default:
				return string.Empty;
		}
	}

	public static (string like, string dislike) GetRandomPreferences()
    {
		(string like, string dislike) preferences;
		preferences.like = Likes.GetRandomElement();
		preferences.dislike = Dislikes.GetRandomElement();

		return preferences; 
    }

	public static Sprite GetRandomAvatar(Species species)
    {
		return GetSpritePool(species)?.GetRandomElement();
    }

	public static Sprite[] GetSpritePool(Species species)
    {
		switch (species)
		{
			case Species.HumanMale:
				return Instance.humanMaleSprites;
			case Species.HumanFemale:
				return Instance.humanFemaleSprites;
			case Species.Oshunian:
			case Species.Helicid:
				return Instance.helicidSprites;
			case Species.Vesta:
				return Instance.vestaSprites;
			case Species.Robot:
				return Instance.robotSprites;
			default:
				return null;
		}
	}
}
