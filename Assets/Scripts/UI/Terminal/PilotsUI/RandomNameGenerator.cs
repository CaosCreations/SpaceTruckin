using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RandomNameGenerator : MonoBehaviour
{
	private enum NameCategory 
	{
		HumanMale, HumanFemale, Helicid, Oshunian, Vesta, Robot 
	};

	private int robotPrefixLength = 3;
	private int robotSuffixLength = 4;

	/// <summary>
		/// Name formats:
			///		Human - [FirstName] [Initial].
			///		Helicid - [LastName] [Initial]. 
			///		Oshunian - [FirstName] [Title]
			///		Vesta - [Prefix]-[Name]
			///		Robot - [Alphabetical prefix]-[Numerical suffix]
			///		
		/// 
	/// </summary>

	public void Start()
	{
	}

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

	private string GenerateName(NameCategory nameCategory)
	{
		switch (nameCategory)
        {
            case NameCategory.HumanMale:
				var maleFirstName = GenerateNamePortion(PilotNameDataSingleton.Instance.HumanMaleNames);
				var maleSurname = GenerateInitial(); 
                return $"{maleFirstName} {maleSurname}.";

			case NameCategory.HumanFemale:
				var femaleFirstName = GenerateNamePortion(PilotNameDataSingleton.Instance.HumanFemaleNames);
				var femaleSurname = GenerateInitial(); 
				return  $"{femaleFirstName} {femaleSurname}.";

			case NameCategory.Helicid:
				var helicidSurname = GenerateNamePortion(PilotNameDataSingleton.Instance.HelicidNames);
				var helicidFirstName = GenerateInitial(); 

				// Helicid surnames come first 
				return $"{helicidSurname} {helicidFirstName}.";

			case NameCategory.Oshunian:
				var oshunianFirstName = GenerateNamePortion(PilotNameDataSingleton.Instance.OshunianNames);
				var oshunianSurname = GenerateNamePortion(PilotNameDataSingleton.Instance.OshunianTitles);

				// No space required since the surname is a title with a space built in
				return $"{oshunianFirstName}{oshunianSurname}";

			case NameCategory.Vesta:
				var vestaPefix = GenerateNamePortion(PilotNameDataSingleton.Instance.VestaPrefixes);
				var vestaName = GenerateNamePortion(PilotNameDataSingleton.Instance.VestaNames); 
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
