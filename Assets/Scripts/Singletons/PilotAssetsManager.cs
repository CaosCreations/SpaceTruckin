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
            Task.Factory.StartNew(() => HumanMaleNames = LoadTextPoolAsync(PilotsConstants.HumanMaleNamesPath).Result),
            Task.Factory.StartNew(() => HumanFemaleNames = LoadTextPoolAsync(PilotsConstants.HumanFemaleNamesPath).Result),
            Task.Factory.StartNew(() => HelicidNames = LoadTextPoolAsync(PilotsConstants.HelicidNamesPath).Result),
            Task.Factory.StartNew(() => OshunianNames = LoadTextPoolAsync(PilotsConstants.OshunianNamesPath).Result),
            Task.Factory.StartNew(() => OshunianTitles = LoadTextPoolAsync(PilotsConstants.OshunianTitlesPath).Result),
            Task.Factory.StartNew(() => VestaPrefixes = LoadTextPoolAsync(PilotsConstants.VestaPrefixesPath).Result),
            Task.Factory.StartNew(() => VestaNames = LoadTextPoolAsync(PilotsConstants.VestaNamesPath).Result),
            Task.Factory.StartNew(() => Likes = LoadTextPoolAsync(PilotsConstants.PilotLikesPath).Result),
            Task.Factory.StartNew(() => Dislikes = LoadTextPoolAsync(PilotsConstants.PilotDislikesPath).Result)
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

				for (int i = 0; i < PilotsConstants.RobotPrefixLength; i++)
				{
					robotPrefix += GenerateInitial();
				}
				for (int i = 0; i < PilotsConstants.RobotSuffixLength; i++)
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
