using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PilotAssetsManager : MonoBehaviour
{
	public static PilotAssetsManager Instance { get; private set; }

	/// <summary>
	/// Maps bundled asset file names to string arrays 
	/// </summary>
	public static Dictionary<string, string[]> PilotTextData { get; private set; } = new Dictionary<string, string[]>()
	{
		// Pilot names 
		{ PilotsConstants.HumanMaleNamesKey, default },
		{ PilotsConstants.HumanFemaleNamesKey, default },
		{ PilotsConstants.HelicidNamesKey, default },
		{ PilotsConstants.OshunianFirstNamesKey, default },
		{ PilotsConstants.OshunianTitlesKey, default },
		{ PilotsConstants.VestaPrefixesKey, default },
		{ PilotsConstants.VestaNamesKey, default },

		// Pilot preferences 
		{ PilotsConstants.PilotLikesKey, default },
		{ PilotsConstants.PilotDislikesKey, default }
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

		pilotTextBundle = bundleRequest?.assetBundle;

		if (pilotTextBundle == null)
		{
			Debug.LogError("Failed to load pilot text asset bundle.");
			yield break;
		}

		// Load all individual assets from bundle 
		AssetBundleRequest assetsRequest = pilotTextBundle.LoadAllAssetsAsync();
		yield return assetsRequest;

		Object[] allAssets = assetsRequest?.allAssets;

		// Cast all assets to TextAssets and split them into arrays that map to PilotTextData
		foreach (Object asset in allAssets)
        {
			if (asset != null)
            {
				TextAsset textAsset = asset as TextAsset;
				PilotTextData[textAsset.name] = textAsset.text.RemoveCarriageReturns().Split('\n');
            }
        }

		// Fire event once loading is complete 
		OnPilotTextDataLoaded?.Invoke();
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
				var maleFirstName = PilotTextData[PilotsConstants.HumanMaleNamesKey].GetRandomElement();
				var maleSurname = GenerateInitial();
				return $"{maleFirstName} {maleSurname}.";

			case Species.HumanFemale:
				var femaleFirstName = PilotTextData[PilotsConstants.HumanFemaleNamesKey].GetRandomElement();
				var femaleSurname = GenerateInitial();
				return $"{femaleFirstName} {femaleSurname}.";

			case Species.Helicid:
				var helicidSurname = PilotTextData[PilotsConstants.HelicidNamesKey].GetRandomElement();
				var helicidFirstName = GenerateInitial();

				// Helicid surnames come first 
				return $"{helicidSurname} {helicidFirstName}.";

			case Species.Oshunian:
				var oshunianFirstName = PilotTextData[PilotsConstants.OshunianFirstNamesKey].GetRandomElement();
				var oshunianSurname = PilotTextData[PilotsConstants.OshunianTitlesKey].GetRandomElement();

				// No space required since the surname is a title with a space built in
				return $"{oshunianFirstName}{oshunianSurname}";

			case Species.Vesta:
				var vestaPefix = PilotTextData[PilotsConstants.VestaPrefixesKey].GetRandomElement();
				var vestaName = PilotTextData[PilotsConstants.VestaNamesKey].GetRandomElement();
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
		preferences.like = PilotTextData[PilotsConstants.PilotLikesKey].GetRandomElement();
		preferences.dislike = PilotTextData[PilotsConstants.PilotDislikesKey].GetRandomElement();

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
