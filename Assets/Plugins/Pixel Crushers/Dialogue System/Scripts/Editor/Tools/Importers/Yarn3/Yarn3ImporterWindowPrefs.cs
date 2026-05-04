#if USE_YARN3

using System;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.Yarn3
{
    [Serializable]
    public class Yarn3ImporterWindowPrefs : AbstractConverterWindowPrefs
    {

        /// <summary>
        /// The name of the player's actor.
        /// </summary>
        public string playerName = Yarn3ImporterPrefs.DefaultPlayerName;

        /// <summary>
        /// The regular expression used to extract actor names from Yarn strings.
        /// </summary>
        public string actorRegex = Yarn3ImporterPrefs.DefaultActorRegex;

        /// <summary>
        /// The regular expression used to strip line prefixes from Yarn strings.
        /// </summary>
        public string linePrefixRegex = Yarn3ImporterPrefs.DefaultLinePrefixRegex;

        /// <summary>
        /// List of all Yarn source files for the project.
        /// </summary>
        public List<string> sourceFiles = new List<string>();

        /// <summary>
        /// The regular expression used to extract locale from localized string filenames.
        /// May possibly want to expose this to the user? Not really sure, probably best not to,
        /// since the Yarn Unity tool creates these filenames and expects a specific pattern.
        /// </summary>
        public string localeRegex = Yarn3ImporterPrefs.DefaultLocaleRegex;

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

        public bool oneConversationPerFile = false;

        public bool keepExistingActors = false;

        public bool debug = false;

        public Yarn3ImporterPrefs ToYarn3ImporterPrefs()
        {
            var prefs = new Yarn3ImporterPrefs();
            prefs.actorRegex = actorRegex;
            prefs.linePrefixRegex = linePrefixRegex;
            prefs.sourceFiles = sourceFiles;
            prefs.playerName = playerName;
            prefs.localeRegex = localeRegex;
            prefs.localizedStringFiles = localizedStringFiles;
            prefs.portraitFolder = portraitFolder;
            prefs.prefsPath = prefsPath;
            prefs.importMenuText = importMenuText;
            prefs.oneConversationPerFile = oneConversationPerFile;
            prefs.keepExistingActors = keepExistingActors;
            prefs.debug = debug;
            prefs.sourceFilename = sourceFilename;
            prefs.outputFolder = outputFolder;
            prefs.databaseFilename = databaseFilename;
            prefs.overwrite = overwrite;
            prefs.merge = merge;
            prefs.encodingType = encodingType;
            return prefs;
        }
    }

}

#endif
