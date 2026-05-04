#if USE_YARN3

using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem.Yarn3
{

    /// <summary>
    /// Utility class to convert Yarn files to dialogue database.
    /// </summary>
    public static class YarnRuntimeImporter
    {

        /// <summary>
        /// Creates a new dialogue database instance using the specified baseID and the Yarn
        /// import settings specified in prefs.
        /// </summary>
        public static DialogueDatabase ConvertYarnToDialogueDatabase(int baseID, Yarn3ImporterPrefs prefs)
        {
            var yarnReader = new YarnImporterProjectReader();
            var yarnProject = yarnReader.Parse(prefs);
            var database = DatabaseUtility.CreateDialogueDatabaseInstance();
            database.baseID = baseID;
            var yarnWriter = new YarnImporterProjectWriter();
            yarnWriter.Write(prefs, yarnProject, database);
            return database;
        }

        /// <summary>
        /// Creates a new dialogue database instance using the specified baseID and
        /// Yarn import settings.
        /// </summary>
        public static DialogueDatabase ConvertYarnToDialogueDatabase(
            int baseID,
            string playerName,
            string actorRegex,
            string linePrefixRegex,
            List<string> sourceFiles,
            EncodingType encodingType,
            List<string> localizedStringFiles,
            bool importMenuText,
            bool oneConversationPerFile,
            bool debug)
        {
            var prefs = new Yarn3ImporterPrefs();
            prefs.playerName = playerName;
            prefs.actorRegex = actorRegex;
            prefs.linePrefixRegex = linePrefixRegex;
            prefs.sourceFiles = sourceFiles;
            prefs.encodingType = encodingType;
            prefs.localizedStringFiles = localizedStringFiles;
            prefs.importMenuText = importMenuText;
            prefs.oneConversationPerFile = oneConversationPerFile;
            prefs.debug = debug;
            return ConvertYarnToDialogueDatabase(baseID, prefs);
        }
    }

}

#endif
