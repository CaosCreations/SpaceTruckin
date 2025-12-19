#if USE_TIMELINE
// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#if USE_ADDRESSABLES
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif
#endif

namespace PixelCrushers.DialogueSystem
{

    public static class ConversationTimelineUtility
    {

        /// <summary>
        /// Returns a best guess of what the dialogue text will be.
        /// </summary>
        /// <param name="conversationTitle">Conversation (or bark conversation) started.</param>
        /// <param name="startingEntryID">Entry started, or -1 for beginning of conversation.</param>
        /// <param name="numContinues">Number of nodes to continue past.</param>
        /// <returns></returns>
        private static void GetDialogueEntry(string conversationTitle, int startingEntryID, int numContinues,
            out DialogueEntry entry, out bool isPlayer)
        {
            entry = null;
            isPlayer = false;
            var dialogueManager = PixelCrushers.GameObjectUtility.FindFirstObjectByType<DialogueSystemController>();
            if (dialogueManager != null && dialogueManager.initialDatabase != null)
            {
                var database = dialogueManager.initialDatabase;
                var conversation = database.GetConversation(conversationTitle);
                if (conversation != null)
                {
                    if (startingEntryID == -1)
                    {
                        var startNode = conversation.GetFirstDialogueEntry();
                        if (startNode != null && startNode.outgoingLinks.Count > 0)
                        {
                            entry = database.GetDialogueEntry(startNode.outgoingLinks[0]);
                        }
                    }
                    else
                    {
                        entry = database.GetDialogueEntry(conversation.id, startingEntryID);
                    }
                    for (int i = 0; i < numContinues; i++)
                    {
                        if (entry == null) break;
                        if (entry.outgoingLinks.Count == 0) { entry = null; break; }
                        entry = database.GetDialogueEntry(entry.outgoingLinks[0]);
                        int safeguard = 0;
                        while (entry != null && entry.isGroup && entry.outgoingLinks.Count > 0 && safeguard++ < 9999)
                        { 
                            // Bypass group links:
                            entry = database.GetDialogueEntry(entry.outgoingLinks[0]);
                        }
                    }
                }
            }
            isPlayer = false;
            if (entry != null && dialogueManager != null && dialogueManager.initialDatabase != null)
            {
                var actorID = entry.ActorID;
                var actor = dialogueManager.initialDatabase.actors.Find(x => x.id == actorID);
                if (actor != null && actor.IsPlayer) isPlayer = true;
            }
        }

        /// <summary>
        /// Returns a best guess of what the dialogue text will be.
        /// </summary>
        /// <param name="conversationTitle">Conversation (or bark conversation) started.</param>
        /// <param name="startingEntryID">Entry started, or -1 for beginning of conversation.</param>
        /// <param name="numContinues">Number of nodes to continue past.</param>
        /// <returns></returns>
        public static string GetDialogueText(string conversationTitle, int startingEntryID, int numContinues = 0)
        {
            DialogueEntry entry;
            bool isPlayer;
            GetDialogueEntry(conversationTitle, startingEntryID, numContinues, out entry, out isPlayer);
            return (entry != null) ? (!string.IsNullOrEmpty(entry.MenuText) ? entry.MenuText : entry.DialogueText)
                : "(determined at runtime)";
        }

        /// <summary>
        /// Returns a best guess of what the sequence will be. Replaces these keywords with
        /// best-guess values:
        /// - {{default}}
        /// - {{end}}
        /// - entrytag / entrytaglocal
        /// </summary>
        /// <param name="conversationTitle">Conversation (or bark conversation) started.</param>
        /// <param name="startingEntryID">Entry started, or -1 for beginning of conversation.</param>
        /// <param name="numContinues">Number of nodes to continue past.</param>
        /// <returns></returns>
        public static string GetSequence(string conversationTitle, int startingEntryID,
            out DialogueEntry entry, int numContinues = 0)
        {
            bool isPlayer;
            string sequence = string.Empty;
            GetDialogueEntry(conversationTitle, startingEntryID, numContinues, out entry, out isPlayer);
            if (entry == null) return string.Empty;
            if (string.IsNullOrEmpty(entry.Sequence))
            {
                sequence = GetDefaultSequence(isPlayer);
            }
            else
            {
                sequence = entry.Sequence;
                if (sequence.Contains("{{default}}"))
                {
                    sequence = sequence.Replace("{{default}}", GetDefaultSequence(isPlayer));
                }
            }
            if (sequence.Contains("entrytaglocal"))
            {
                sequence = sequence.Replace("entrytaglocal", GetEntrytag(entry));
            }
            else if (sequence.Contains("entrytag"))
            {
                sequence = sequence.Replace("entrytag", GetEntrytag(entry));
            }
            if (sequence.Contains("{{end}}"))
            {
                sequence = sequence.Replace("{{end}}", ConversationView.GetDefaultSubtitleDurationInSeconds(entry.DialogueText).ToString());
            }
            return sequence;
        }

        private static string GetEntrytag(DialogueEntry entry)
        {
            if (entry == null) return string.Empty;
            var dialogueManager = PixelCrushers.GameObjectUtility.FindFirstObjectByType<DialogueSystemController>();
            if (dialogueManager == null || dialogueManager.initialDatabase == null) return "entrytag";
            return dialogueManager.initialDatabase.GetEntrytag(entry.conversationID, entry.id, dialogueManager.displaySettings.cameraSettings.entrytagFormat);
        }

        private static string GetDefaultSequence(bool isPlayer)
        {
            var dialogueManager = PixelCrushers.GameObjectUtility.FindFirstObjectByType<DialogueSystemController>();
            if (dialogueManager == null) return "Delay({{end}})"; // default fallback value.
            if (isPlayer && !string.IsNullOrEmpty(dialogueManager.displaySettings.cameraSettings.defaultPlayerSequence))
            {
                return dialogueManager.displaySettings.cameraSettings.defaultPlayerSequence;
            }
            return dialogueManager.displaySettings.cameraSettings.defaultSequence;
        }

        private const float MinSequenceDuration = 0.5f;
        private const float DefaultSequenceDuration = 1;
        private const float MinSubtitleSeconds = 1;
        private static bool hasLookedForTypewriter = false;
        private static float typewriterCharsPerSecond = 50;

        public static float GetSequenceDuration(string conversationTitle, int startingEntryID, int numContinues = 0)
        {
            DialogueEntry entry;
            var sequence = GetSequence(conversationTitle, startingEntryID, out entry, numContinues);
            if (sequence == null) return DefaultSequenceDuration;
            var audioLength = GetAudioLength(sequence);
            var delayLength = GetDelayLength(sequence);
            var typedLength = GetTypewriterLength(entry.DialogueText, sequence);
            //Debug.Log($"{conversationTitle}:{startingEntryID}+{numContinues} audio={audioLength}, delay={delayLength}, typed={typedLength}");
            return Mathf.Max(MinSequenceDuration, audioLength, delayLength, typedLength);
        }

        /// <summary>
        /// Return duration of Delay() command, or 0 if no Delay() in sequence.
        /// </summary>
        private static float GetDelayLength(string sequence)
        {
            if (string.IsNullOrEmpty(sequence)) return 0;
            var pos = sequence.IndexOf("Delay(");
            if (pos == -1) return 0;
            pos += "Delay(".Length;
            var rPos = sequence.IndexOf(")", pos);
            if (rPos == -1) return 0;
            var parameter = sequence.Substring(pos, rPos - pos);
            return float.TryParse(parameter, out var value) ? value: 0;
        }

        /// <summary>
        /// Return duration of typewriter effect.
        /// </summary>
        private static float GetTypewriterLength(string text, string sequence)
        {
            if (string.IsNullOrEmpty(sequence)) return 0;
            if (!sequence.Contains("Typed")) return 0;
            if (!hasLookedForTypewriter)
            {
                hasLookedForTypewriter = true;
                AbstractTypewriterEffect typewriterEffect = null;
                var dialogueManager = PixelCrushers.GameObjectUtility.FindFirstObjectByType<DialogueSystemController>();
                if (dialogueManager != null)
                {
                    var ui = DialogueManager.dialogueUI as StandardDialogueUI;
                    if (ui != null && ui.conversationUIElements.defaultNPCSubtitlePanel != null &&
                        ui.conversationUIElements.defaultNPCSubtitlePanel.subtitleText != null)
                    {
                        typewriterEffect = ui.conversationUIElements.defaultNPCSubtitlePanel.subtitleText.gameObject.GetComponent<AbstractTypewriterEffect>();
                    }
                }
                if (typewriterEffect == null) typewriterEffect = PixelCrushers.GameObjectUtility.FindFirstObjectByType<AbstractTypewriterEffect>();
                if (typewriterEffect != null) typewriterCharsPerSecond = typewriterEffect.charactersPerSecond;
            }

            int numCharacters = string.IsNullOrEmpty(text) ? 0 : Tools.StripRichTextCodes(text).Length;
            float totalRPGMakerPauseLength = 0;
            if (text.Contains("\\"))
            {
                var numFullPauses = (text.Length - text.Replace("\\.", string.Empty).Length) / 2;
                var numQuarterPauses = (text.Length - text.Replace("\\,", string.Empty).Length) / 2;
                totalRPGMakerPauseLength = (1.0f * numFullPauses) + (0.25f * numQuarterPauses);
            }
            return totalRPGMakerPauseLength + (numCharacters / Mathf.Max(1, typewriterCharsPerSecond));
        }

        private static float GetAudioLength(string sequence)
        {
            if (sequence.Contains("AudioWait("))
            {
                return GetAudioLength("AudioWait(", sequence);
            }
            else if (sequence.Contains("SALSA("))
            {
                return GetAudioLength("SALSA(", sequence);
            }
            else
            {
                return 0;
            }
        }

        private static float GetAudioLength(string command, string sequence)
        {
            if (string.IsNullOrEmpty(sequence)) return DefaultSequenceDuration;
            var pos1 = sequence.IndexOf(command) + command.Length;
            var posParen = sequence.IndexOf(")", pos1 + 1);
            var posComma = sequence.IndexOf(",", pos1 + 1);
            var pos2 = sequence.Length;
            if (posParen != -1) pos2 = posParen;
            if (posComma != -1 && posComma < posParen) pos2 = posComma;
            var audioFileName = sequence.Substring(pos1, pos2 - pos1).Trim();
                var audioClip = LoadAudioClip(audioFileName);
                if (audioClip != null) return audioClip.length;
                return DefaultSequenceDuration;
        }

        private static AudioClip LoadAudioClip(string audioFileName)
        {
#if UNITY_EDITOR
            AudioClip audioClip = Resources.Load<AudioClip>(audioFileName);
            if (audioClip != null) return audioClip;

#if USE_ADDRESSABLES
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var allEntries = new List<AddressableAssetEntry>(settings.groups.SelectMany(g => g.entries));
            var foundEntry = allEntries.FirstOrDefault(e => e.address == audioFileName);
            if (foundEntry != null) audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(foundEntry.AssetPath);
            if (audioClip != null) return audioClip;
#endif
#endif
            return null;
        }

    }
}
#endif
