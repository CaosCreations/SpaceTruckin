using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class PilotAssetsManager : MonoBehaviour
{
	public static PilotAssetsManager Instance { get; private set; }

	// Pilot names  
	public static string[] HumanMaleNames { get; private set; }
	public static string[] HumanFemaleNames { get; private set; }
	public static string[] HelicidNames { get; private set; }
	public static string[] OshunianNames { get; private set; }
	public static string[] OshunianTitles { get; private set; }
	public static string[] VestaPrefixes { get; private set; }
	public static string[] VestaNames { get; private set; }

	/// <summary>
	/// Maps bundled asset names to string array identifiers
	/// </summary>
	public static Dictionary<string, string[]> PilotTextData = new Dictionary<string, string[]>()
	{
		{ PilotsConstants.HumanMaleNames, HumanMaleNames },
		{ PilotsConstants.HumanFemaleNames, HumanFemaleNames },
		{ PilotsConstants.HelicidNames, HelicidNames },
		{ PilotsConstants.OshunianNames, OshunianNames },
		{ PilotsConstants.OshunianTitles, OshunianTitles },
		{ PilotsConstants.VestaPrefixes, VestaPrefixes },
		{ PilotsConstants.VestaNames, VestaNames }
	};

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

	private static AssetBundle pilotTextBundle;

	public static event System.Action OnPilotTextDataLoaded;

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

	public void Init()
    {
		StartCoroutine(LoadTextAssetsFromBundle());
    }

    private IEnumerator LoadTextAssetsFromBundle()
    {
		// Load pilot text asset bundle "asynchronously" and store reference 
		AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(
			Path.Combine(PilotsConstants.BundleLoadingPath, PilotsConstants.PilotTextBundleName));

		yield return bundleRequest;

		pilotTextBundle = bundleRequest.assetBundle;

		if (pilotTextBundle == null)
		{
			Debug.LogError("Failed to load pilot text asset bundle.");
			yield break;
		}

		// Load all individual assets from bundle 
		AssetBundleRequest assetsRequest = pilotTextBundle.LoadAllAssetsAsync();
		yield return assetsRequest;

		Object[] allAssets = assetsRequest.allAssets;

		// Cast all assets to TextAssets and split them into arrays that map to PilotTextData
		foreach (Object asset in allAssets)
        {
			TextAsset textAsset = asset as TextAsset;
			PilotTextData[textAsset.name] = textAsset.text.Split('\n');
        }

		// Fire event once loading is complete 
		OnPilotTextDataLoaded?.Invoke();
	}

	// Combine the value returned with an initial, digit, or second portion
	private static string GenerateNamePortion(string[] namePool)
	{
		return namePool[random.Next(0, namePool.Length)];
	}

	private static char GenerateInitial()
	{
		return char.ToUpper((char)('a' + random.Next(0, 26)));
	}

	private static int GenerateDigit()
	{
		return random.Next(0, 9);
	}

	public static string GetRandomName(Species species)
	{
		switch (species)
		{
			case Species.HumanMale:
				var maleFirstName = GenerateNamePortion(HumanMaleNames);
				var maleSurname = GenerateInitial();
				return $"{maleFirstName} {maleSurname}.";

			case Species.HumanFemale:
				var femaleFirstName = GenerateNamePortion(HumanFemaleNames);
				var femaleSurname = GenerateInitial();
				return $"{femaleFirstName} {femaleSurname}.";

			case Species.Helicid:
				var helicidSurname = GenerateNamePortion(HelicidNames);
				var helicidFirstName = GenerateInitial();

				// Helicid surnames come first 
				return $"{helicidSurname} {helicidFirstName}.";

			case Species.Oshunian:
				var oshunianFirstName = GenerateNamePortion(OshunianNames);
				var oshunianSurname = GenerateNamePortion(OshunianTitles);

				// No space required since the surname is a title with a space built in
				return $"{oshunianFirstName}{oshunianSurname}";

			case Species.Vesta:
				var vestaPefix = GenerateNamePortion(VestaPrefixes);
				var vestaName = GenerateNamePortion(VestaNames);
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
