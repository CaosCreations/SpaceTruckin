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

    /// <summary>
    /// Import window for Yarn 3 files.
    /// - The import process uses YarnImporterProjectReader to read the specified
    ///   yarn files and return a YarnImporterProject.
    /// - YarnImporterProject contains a list of ConversationNodes which represent
    ///   Yarn nodes / Dialogue System conversations, and localized strings.
    /// - Then the import process uses a YarnImporterProjectWriter to populate the
    ///   dialogue database with:
    ///   - Actors referenced in nodes.
    ///   - Variables referenced in set or declare statements.
    ///   - Conversations from ConversationNodes.
    /// </summary>
    public class Yarn3ImporterWindow : AbstractConverterWindow<Yarn3ImporterWindowPrefs>
    {

        [MenuItem("Tools/Pixel Crushers/Dialogue System/Import/Yarn/Yarn 3...", false, 1)]
        public static void Init()
        {
            var window = EditorWindow.GetWindow(typeof(Yarn3ImporterWindow), false, "Yarn 3 Importer");
            window.minSize = new Vector2(400, 400);
        }

        public override string sourceFileExtension { get { return "yarn"; } }

        public const string PrefsKeyName = "PixelCrushers.DialogueSystem.YarnConverterSettings";
        public override string prefsKey { get { return PrefsKeyName; } }

        protected static GUIContent SavePrefsLabel = new GUIContent("Save Prefs...", "Save import settings to JSON file.");
        protected static GUIContent LoadPrefsLabel = new GUIContent("Load Prefs...", "Load import settings from JSON file.");

        public static readonly string[] YarnExampleFiles = new string[]
        {
            // SallyPath,
            // ShipPath
            "Assets/Samples/Yarn Spinner/1.2.7/Space/Dialogue/Sally.yarn",
            "Assets/Samples/Yarn Spinner/1.2.7/Space/Dialogue/Ship.yarn",
        };

        public static readonly string[] RuntimeTestFiles = new string[]
        {
            // "Assets/Test/Yarn/RuntimeTestScripts/AllOperatorsTest.yarn",
            "Assets/Test/Yarn/RuntimeTestScripts/RunCommand.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/FormattedRunLineFromStart.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/ShortcutOptionToRunNode.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/UnreachableCodeFromRunNode.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/UnreachableCodeFromStop.yarn",
            // "Assets/Test/Yarn/RuntimeTestScripts/Wait.yarn",
        };

        public static readonly string[] SyntaxTestFiles = new string[]
        {
            // "Assets/Test/Yarn/YarnExamples/SyntaxTest.yarn",
            "Assets/Test/Yarn/YarnExamples/Sally_Test.yarn",
            "Assets/Test/Yarn/YarnExamples/Ship_Test.yarn",
        };

        public static readonly string[] RopeworksYarnFiles = new string[]
        {
            // "Assets/Test/Yarn/Ropeworks/RopeworkBlank.yarn",
            "Assets/Test/Yarn/Ropeworks/RopeworkComplexExample.yarn",
            "Assets/Test/Yarn/Ropeworks/RopeworkSimpleExample.yarn",
            // "Assets/Test/Yarn/Ropeworks/RopeworksTestScript.yarn",
        };

        public static readonly string[] CustomerTestFiles = new string[]
        {
            "Assets/Test/Yarn/CustomerTest/Carla.yarn",
            "Assets/Test/Yarn/CustomerTest/Estatua.yarn",
            "Assets/Test/Yarn/CustomerTest/Hector.yarn",
            "Assets/Test/Yarn/CustomerTest/Livro.yarn",
            "Assets/Test/Yarn/CustomerTest/Peixoto.yarn",
            "Assets/Test/Yarn/CustomerTest/Tuto.yarn",
        };

        public static readonly string[] SpecialTestCaseFiles = new string[]
        {
            "Assets/Test/Yarn/SpecialTestCases/ConditionalShortcutOptions.yarn",
        };

        // public const string StanleyParableExample = "Assets/Test/Yarn/StanleyParable/StanleyParable.yarn";
        public static readonly string[] StanleyParableYarnFiles = new string[]
        {
            // StanleyParableExample // Has syntax error
            "Assets/Test/Yarn/StanleyParable/StanleyParable.yarn",
        };

        public static readonly string[] YarnSpinnerTestFiles = new string[]
        {
            // // AnalysisTest,
            // // Basic,
            // // Compiler,
            // // Example,
            // // Headers,
            // // InvalidNodeTitle, -- Illegal syntax
            // // Options,
            // RandomOptions,
            // // SkippedOptions,
            // // Strings,
            // // Tagging, -- Illegal syntax
            // "Assets/Test/Yarn/YarnSpinnerTests/AnalysisTest.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Basic.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Compiler.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Example.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Headers.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/InvalidNodeTitle.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Options.yarn",
            "Assets/Test/Yarn/YarnSpinnerTests/RandomOptions.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/SkippedOptions.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Strings.yarn",
            // "Assets/Test/Yarn/YarnSpinnerTests/Tagging.yarn",
        };

        private ReorderableList uiYarnSourceFileList;
        private ReorderableList uiLocalizedFileList;
        private string lastVisitedYarnSourceDirectory;
        private string lastVisitedLocalizedFileDirectory;

        public void PopulateDefaultPrefs()
        {
            prefs.playerName = Yarn3ImporterPrefs.DefaultPlayerName;
            prefs.actorRegex = Yarn3ImporterPrefs.DefaultActorRegex;
            prefs.linePrefixRegex = Yarn3ImporterPrefs.DefaultLinePrefixRegex;
            prefs.localeRegex = Yarn3ImporterPrefs.DefaultLocaleRegex;
            // prefs.customCommandsSourceFile = YarnConverterPrefs.DefaultCustomCommandsSourceFile;

            prefs.sourceFiles = new List<string>();
            // prefs.sourceFiles = new List<string>(SyntaxTestFiles);
            // prefs.sourceFiles = new List<string>(RuntimeTestFiles);
            // prefs.sourceFiles = new List<string>(YarnExampleFiles);
            // prefs.sourceFiles = new List<string>(RopeworksYarnFiles);
            // prefs.sourceFiles = new List<string>(CustomerTestFiles);

            prefs.localizedStringFiles = new List<string>();
            //{
            //    "Assets/Samples/Yarn Spinner/1.2.7/Space/Dialogue/Sally (de).csv",
            //    "Assets/Samples/Yarn Spinner/1.2.7/Space/Dialogue/Ship (de).csv"
            //};

            prefs.outputFolder = "Assets";
            prefs.databaseFilename = "Dialogue Database";
            prefs.overwrite = true;
            prefs.merge = false;
            prefs.encodingType = EncodingType.Default;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            // PopulateDefaultPrefs(); // Comment this out for production, only use for testing

            InitializeReorderableLists();
        }

        protected void InitializeReorderableLists()
        {
            uiYarnSourceFileList = new ReorderableList(prefs.sourceFiles, typeof(string), true, true, true, true);
            uiYarnSourceFileList.drawHeaderCallback += DrawYarnSourceFileListHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.
            uiYarnSourceFileList.drawElementCallback += DrawYarnSourceFileListItem; // Delegate to draw the elements on the list
            uiYarnSourceFileList.onAddCallback += AddYarnSourceFileListItem; // Add stuff
            lastVisitedYarnSourceDirectory = (prefs.sourceFiles.Count > 0) ? Path.GetDirectoryName(prefs.sourceFiles[prefs.sourceFiles.Count - 1]) : "Assets";

            uiLocalizedFileList = new ReorderableList(prefs.localizedStringFiles, typeof(string), true, true, true, true);
            uiLocalizedFileList.drawHeaderCallback += DrawLocalizedFileListHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.
            uiLocalizedFileList.drawElementCallback += DrawLocalizedFileListItem; // Delegate to draw the elements on the list
            uiLocalizedFileList.onAddCallback += AddLocalizedFileListItem; // Add stuff
            lastVisitedLocalizedFileDirectory = (prefs.localizedStringFiles.Count > 0) ? Path.GetDirectoryName(prefs.localizedStringFiles[prefs.localizedStringFiles.Count - 1]) : "Assets";
        }

        protected override void DrawControls()
        {
            DrawSourceSection();
            DrawDestinationSection();
            DrawConversionButtons();
        }

        protected static GUIContent PlayerNameLabel = new GUIContent("Player Name", "Actor name to use for default player actor.");
        protected static GUIContent ActorRegexLabel = new GUIContent("Actor Regex", "How to extract actors names from Yarn dialogue lines. Defaults to 'NAME: Dialogue text'.");
        protected static GUIContent LinePrefixRegexLabel = new GUIContent("Line Prefix Regex", "How to extract dialogue text from Yarn dialogue lines. Defaults to 'Ignore-this: Dialogue text'.");
        protected static GUIContent CustomCommandsFileLabel = new GUIContent("Custom Commands File", "Filename to use when auto-generating script containing custom commands referenced in your Yarn stories.");
        protected static GUIContent PortraitFolderLabel = new GUIContent("Portrait Folder", "The importer will look here for actor entities' portrait textures.");
        protected static GUIContent DebugLabel = new GUIContent("Debug", "Log detailed debug info to the Console.");
        protected static GUIContent YarnSourceFilesLabel = new GUIContent("Yarn Source Files", "Yarn files to import.");
        protected static GUIContent LocalizedStringFilesLabel = new GUIContent("Localized String Files", "Yarn localization files.");

        protected override void DrawSourceSection()
        {
            prefs.playerName = EditorGUILayout.TextField(PlayerNameLabel, prefs.playerName)?.Trim();
            prefs.actorRegex = EditorGUILayout.TextField(ActorRegexLabel, prefs.actorRegex);
            prefs.linePrefixRegex = EditorGUILayout.TextField(LinePrefixRegexLabel, prefs.linePrefixRegex);

            // prefs.customCommandsSourceFile = EditorGUILayout.TextField(CustomCommandsFileLabel, prefs.customCommandsSourceFile)?.Trim();
            uiYarnSourceFileList.DoLayoutList();
            uiLocalizedFileList.DoLayoutList();
            DrawPortraitFolderField();
        }

        private void DrawPortraitFolderField()
        {
            EditorGUILayout.BeginHorizontal();
            prefs.portraitFolder = EditorGUILayout.TextField(PortraitFolderLabel, prefs.portraitFolder);
            if (GUILayout.Button("...", EditorStyles.miniButtonRight, GUILayout.Width(22)))
            {
                prefs.portraitFolder = EditorUtility.OpenFolderPanel("Location of Portrait Textures", prefs.portraitFolder, "");
                prefs.portraitFolder = "Assets" + prefs.portraitFolder.Replace(Application.dataPath, string.Empty);
                GUIUtility.keyboardControl = 0;
            }
            EditorGUILayout.EndHorizontal();
        }

        void DrawYarnSourceFileListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, YarnSourceFilesLabel);
        }

        void DrawYarnSourceFileListItem(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index < 0 || index >= prefs.sourceFiles.Count)
            {
                return;
            }

            prefs.sourceFiles[index] = EditorGUI.TextField(new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight), GUIContent.none, prefs.sourceFiles[index]);
        }

        private void AddYarnSourceFileListItem(ReorderableList list)
        {
            var fullPath = EditorUtility.OpenFilePanel("Select Yarn File", lastVisitedYarnSourceDirectory, "yarn");
            if (string.IsNullOrEmpty(fullPath)) return;

            // Truncate this if it's in the assets directory, it makes things way easier to read
            var displayPath = fullPath;
            if (fullPath.StartsWith(Application.dataPath))
            {
                displayPath = "Assets" + fullPath.Substring(Application.dataPath.Length);
            }

            if (!prefs.sourceFiles.Contains(displayPath))
            {
                prefs.sourceFiles.Add(displayPath);
                lastVisitedYarnSourceDirectory = Path.GetDirectoryName(fullPath);
            }
        }

        private void DrawLocalizedFileListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, LocalizedStringFilesLabel);
        }

        private void DrawLocalizedFileListItem(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index < 0 || index >= prefs.localizedStringFiles.Count)
            {
                return;
            }

            prefs.localizedStringFiles[index] = EditorGUI.TextField(new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight), GUIContent.none, prefs.localizedStringFiles[index]);
        }

        private void AddLocalizedFileListItem(ReorderableList list)
        {
            var fullPath = EditorUtility.OpenFilePanel("Select localized file", lastVisitedLocalizedFileDirectory, "csv");
            if (string.IsNullOrEmpty(fullPath)) return;

            // Truncate this if it's in the assets directory, it makes things way easier to read
            var displayPath = fullPath;
            if (fullPath.StartsWith(Application.dataPath))
            {
                displayPath = "Assets" + fullPath.Substring(Application.dataPath.Length);
            }

            if (!prefs.localizedStringFiles.Contains(displayPath))
            {
                prefs.localizedStringFiles.Add(displayPath);
                lastVisitedLocalizedFileDirectory = Path.GetDirectoryName(fullPath);
            }
        }

        protected override void DrawOverwriteCheckbox()
        {
            // Also show import menu text, one conversation per file, and debug checkboxes:
            prefs.importMenuText = EditorGUILayout.Toggle(new GUIContent("Import Menu Text", 
                "If a line has the form '[menutext] dialoguetext', set the dialogue entry's Menu Text to 'menutext' and Dialogue Text to 'dialoguetext'."),
                prefs.importMenuText);

            prefs.oneConversationPerFile = EditorGUILayout.Toggle(new GUIContent("One Conversation Per File", 
                "Each file constitutes one conversation with Yarn nodes as branches in conversation."),
                prefs.oneConversationPerFile);

            prefs.overwrite = EditorGUILayout.Toggle(new GUIContent("Overwrite", 
                "Overwrite database if it already exists. If unticked, then if database already exists the import will create a new database under a new name."),
                prefs.overwrite);

            if (prefs.overwrite)
            {
                EditorGUI.indentLevel++;
                prefs.keepExistingActors = EditorGUILayout.Toggle(new GUIContent("Keep Existing Actors", 
                    "keep existing actors instead of clearing actors list and creating new actors with new IDs."),
                    prefs.keepExistingActors);
                EditorGUI.indentLevel--;
            }

            prefs.debug = EditorGUILayout.Toggle(new GUIContent("Debug", "Log debug info to the console when importing."),
                                                     prefs.debug);
        }

        protected override void DrawConversionButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            DrawSaveLoadPrefsButtons();
            DrawClearButton();
            DrawConvertButton();
            EditorGUILayout.EndHorizontal();
        }

        protected void DrawSaveLoadPrefsButtons()
        {
            if (GUILayout.Button(SavePrefsLabel, GUILayout.Width(100)))
            {
                var path = EditorUtility.SaveFilePanel("Save Import Settings",
                    System.IO.Path.GetDirectoryName(prefs.prefsPath), System.IO.Path.GetFileName(prefs.prefsPath), "json");
                if (!string.IsNullOrEmpty(path))
                {
                    prefs.prefsPath = path;
                    System.IO.File.WriteAllText(path, JsonUtility.ToJson(prefs));
                }
            }
            if (GUILayout.Button(LoadPrefsLabel, GUILayout.Width(100)))
            {
                var path = EditorUtility.OpenFilePanel("Load Import Settings",
                    System.IO.Path.GetDirectoryName(prefs.prefsPath), "json");
                if (!string.IsNullOrEmpty(path))
                {
                    var newPrefs = JsonUtility.FromJson<Yarn3ImporterWindowPrefs>(System.IO.File.ReadAllText(path));
                    if (newPrefs == null)
                    {
                        EditorUtility.DisplayDialog("Load Failed", $"Could not load Yarn import settings from {path}.", "OK");
                    }
                    else
                    {
                        prefs = newPrefs;
                        prefs.prefsPath = path;
                        InitializeReorderableLists();
                    }
                }
            }
        }

        protected override DialogueDatabase LoadOrCreateDatabase()
        {
            string assetPath = string.Format("{0}/{1}.asset", prefs.outputFolder, prefs.databaseFilename);
            DialogueDatabase database = null;
            if (prefs.overwrite)
            {
                database = AssetDatabase.LoadAssetAtPath(assetPath, typeof(DialogueDatabase)) as DialogueDatabase;
                // if ((database != null) && !prefs.merge) database.Clear();
                if (database != null)
                {
                    // We always keep variables so they persistent between imports.
                    // database.variables.Clear();

                    if (!prefs.keepExistingActors)
                    {
                        database.actors.Clear();
                    }
                    database.items.Clear();
                    database.locations.Clear();
                    database.conversations.Clear();
                    database.ResetCache();
                }
            }
            if (database == null)
            {
                assetPath = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", prefs.outputFolder, prefs.databaseFilename));
                database = DatabaseUtility.CreateDialogueDatabaseInstance();
                database.name = prefs.databaseFilename;
                AssetDatabase.CreateAsset(database, assetPath);
            }
            return database;
        }

        protected override void ClearPrefs()
        {
            base.ClearPrefs();
            InitializeReorderableLists();
        }

        protected override bool IsReadyToConvert()
        {
            return
                prefs.sourceFiles.Count > 0 &&
                // !string.IsNullOrEmpty(prefs.customCommandsSourceFile) &&
                !string.IsNullOrEmpty(prefs.databaseFilename) &&
                !string.IsNullOrEmpty(prefs.outputFolder);
        }

        protected override void Convert()
        {
            RunConverter();
        }

        private void WriteDialogueSystemChanges(DialogueDatabase database)
        {
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
        }

        private void RunConverter()
        {
            Debug.Log($"Starting Yarn project import ...");
            var runtimePrefs = prefs.ToYarn3ImporterPrefs();
            var yarnReader = new YarnImporterProjectReader();
            var yarnProject = yarnReader.Parse(runtimePrefs);
            var dialogueDb = LoadOrCreateDatabase();
            var yarnWriter = new YarnImporterProjectWriter();
            yarnWriter.Write(runtimePrefs, yarnProject, dialogueDb);
            WriteDialogueSystemChanges(dialogueDb);
            if (DialogueEditor.DialogueEditorWindow.instance != null) DialogueEditor.DialogueEditorWindow.instance.Reset();
            Debug.Log($"Yarn project import complete - database written to: {AssetDatabase.GetAssetPath(dialogueDb)}", dialogueDb);
        }
    }
}

#endif
