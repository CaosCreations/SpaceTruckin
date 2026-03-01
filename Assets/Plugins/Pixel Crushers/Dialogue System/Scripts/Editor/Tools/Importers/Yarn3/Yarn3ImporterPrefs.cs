#if USE_YARN3

using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using Yarn.Unity;


namespace PixelCrushers.DialogueSystem.Yarn3
{
    [Serializable]
    public class Yarn3ImporterPrefs : AbstractConverterWindowPrefs
    {
        public const string DefaultPlayerName = "Player";
        public const string DefaultActorRegex = "^(.+?)\\:";
        public const string DefaultLinePrefixRegex = "^.+?\\:\\s*";
        public const string DefaultLocaleRegex = ".+\\((.+)\\)\\.csv";

        /// <summary>
        /// The name of the player's actor.
        /// </summary>
        public string playerName = DefaultPlayerName;

        /// <summary>
        /// The regular expression used to extract actor names from Yarn strings.
        /// </summary>
        public string actorRegex = DefaultActorRegex;

        /// <summary>
        /// The regular expression used to strip line prefixes from Yarn strings.
        /// </summary>
        public string linePrefixRegex = DefaultLinePrefixRegex;

        /// <summary>
        /// List of all Yarn source files for the project.
        /// </summary>
        public List<string> sourceFiles = new List<string>();

        /// <summary>
        /// The regular expression used to extract locale from localized string filenames.
        /// May possibly want to expose this to the user? Not really sure, probably best not to,
        /// since the Yarn Unity tool creates these filenames and expects a specific pattern.
        /// </summary>
        public string localeRegex = DefaultLocaleRegex;

        /// <summary>
        /// The list of localized string filenames.
        /// </summary>
        public List<string> localizedStringFiles = new List<string>();

        /// <summary>
        /// Location of portrait images.
        /// </summary>
        public string portraitFolder = "Assets";

        public string prefsPath;

        /// <summary>
        /// If dialogue text starts with text in [square brackets], extract the
        /// text in [square brackets] and assign it to the menu text.
        /// </summary>
        public bool importMenuText = false;

        public bool keepExistingActors = false;

        public bool debug = false;
    }

}

#endif
