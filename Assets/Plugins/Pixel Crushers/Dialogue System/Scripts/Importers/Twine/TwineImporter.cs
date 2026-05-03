#if USE_TWINE
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PixelCrushers.DialogueSystem.Twine
{

    /// <summary>
    /// Imports Twine 2 Harlowe stories exported from Twison
    /// into dialogue database conversations.
    /// </summary>
    public class TwineImporter
    {

        #region Convert

        protected DialogueDatabase database { get; set; }
        protected Template template { get; set; }

        protected HashSet<int> playerActorIDs = new HashSet<int>();

        protected Conversation currentConversation;

        protected class GotoLink
        {
            public DialogueEntry entry;
            public string destinationTitle;
        }

        protected List<GotoLink> gotoLinks = new List<GotoLink>();

        public virtual void ConvertStoryToConversation(DialogueDatabase database, Template template, TwineStory story, int actorID, int conversantID, bool splitPipesIntoEntries, bool useTwineNodePositions = false)
        {
            this.database = database;
            this.template = template;

            // Get/create conversation:
            var conversation = database.GetConversation(story.name);
            if (conversation == null)
            {
                conversation = template.CreateConversation(template.GetNextConversationID(database), story.name);
                database.conversations.Add(conversation);
            }
            conversation.ActorID = actorID;
            conversation.ConversantID = conversantID;
            currentConversation = conversation;
            gotoLinks.Clear();

            // Record player actorIDs:
            playerActorIDs.Clear();
            playerActorIDs.Add(actorID); // Assume actor is Player.
            database.actors.ForEach(actor =>
            {
                if (actor.IsPlayer) playerActorIDs.Add(actor.id);
            });

            // Reset to just <START> node:
            conversation.dialogueEntries.Clear();
            var startEntry = template.CreateDialogueEntry(0, conversation.id, "START");
            conversation.dialogueEntries.Add(startEntry);

            // Find the highest pid:
            int highestPid = 0;
            foreach (var passage in story.passages)
            {
                highestPid = Mathf.Max(highestPid, SafeConvert.ToInt(passage.pid));
            }

            // Since we only create conditional branches based on the syntax
            // (if/else-if/else: ...)[ [[link]] ], we need to replace syntax of the form
            // (if/else-if/else: ...)[(go-to: ...)] with links:
            ReplaceConditionalGotoWithLinks(story);

            // Add passages as dialogue entry nodes:
            var isFirstPassage = true;
            var allHooks = new Dictionary<TwinePassage, List<TwineHook>>();
            foreach (var passage in story.passages)
            {
                var entryID = SafeConvert.ToInt(passage.pid);
                if (entryID == 0) entryID = ++highestPid;
                var entry = template.CreateDialogueEntry(entryID, conversation.id, RemoveParensFromPassageName(passage.name));
                if (useTwineNodePositions)
                {
                    SetEntryPosition(entry, passage.position);
                    if (isFirstPassage)
                    {
                        isFirstPassage = false;
                        SetEntryPosition(startEntry, new TwinePosition(Mathf.Max(1, passage.position.x - DialogueEntry.CanvasRectWidth / 4f), Mathf.Max(1, passage.position.y - 1.5f * DialogueEntry.CanvasRectHeight)));
                    }
                }
                int entryActorID, entryConversantID;
                string dialogueText, sequence, conditions, script, description;
                List<TwineHook> hooks;
                ExtractParticipants(passage.text, actorID, conversantID, false, out dialogueText, out entryActorID, out entryConversantID);
                ExtractSequenceConditionsScriptDescription(ref dialogueText, out sequence, out conditions, out script, out description);
                ExtractHooks(ref dialogueText, out hooks);
                allHooks.Add(passage, hooks);
                dialogueText = RemoveAllLinksFromText(dialogueText);
                ExtractMacros(ref dialogueText, ref entry);
                dialogueText = ReplaceFormatting(dialogueText);
                entry.DialogueText = dialogueText.Trim();
                entry.ActorID = entryActorID;
                entry.ConversantID = conversantID;
                entry.Sequence = AppendCode(entry.Sequence, sequence);
                string falseConditionAction;
                CheckConditionsForPassthrough(conditions, out conditions, out falseConditionAction);
                entry.conditionsString = AppendCode(entry.conditionsString, conditions);
                entry.falseConditionAction = falseConditionAction;
                entry.userScript = AppendCode(entry.userScript, script);
                if (!string.IsNullOrEmpty(description)) description += " ";
                description += $"[pid:{passage.pid}]";
                Field.SetValue(entry.fields, DialogueSystemFields.Description, description);
                conversation.dialogueEntries.Add(entry);
            }

            // Link startnode:
            var startnodeID = SafeConvert.ToInt(story.startnode);
            startEntry.outgoingLinks.Add(new Link(conversation.id, startEntry.id, conversation.id, startnodeID));

            // Link nodes:
            int linkNum = 0;
            foreach (var passage in story.passages)
            {
                if (passage.links == null) continue;
                var originID = SafeConvert.ToInt(passage.pid);
                var originEntry = conversation.GetDialogueEntry(originID);
                foreach (var link in passage.links)
                {
                    if (link == null) continue;
                    var willLinkInHook = IsLinkInHooks(link.link, allHooks[passage]);
                    var linkedPassageID = SafeConvert.ToInt(link.pid);
                    //-- Save for potential future use: var destinationPassageEntry = conversation.GetDialogueEntry(link.link) ?? conversation.GetDialogueEntry(RemoveFormatting(link.link));
                    if (IsLinkImplicit(link))
                    {
                        // Link passages directly with implicit links (with parens around name):
                        if (!willLinkInHook)
                        {
                            originEntry.outgoingLinks.Add(new Link(conversation.id, originID, conversation.id, linkedPassageID));
                        }
                    }
                    else
                    {
                        // Check if there's a node that's a repeat of the link and is assigned to a player actor
                        // (if so, don't add a link entry):
                        var linkEntryTitle = GetLinkEntryTitle(link.link, originID);
                        var linkRepeatEntry = conversation.GetDialogueEntry(linkEntryTitle);
                        if (linkRepeatEntry != null && playerActorIDs.Contains(linkRepeatEntry.ActorID))
                        {
                            // Link links to node for that link (to allow Script: etc in link), so do nothing.
                            var linkEntry = linkRepeatEntry;
                            if (useTwineNodePositions)
                            {
                                SetEntryPosition(linkEntry, new TwinePosition(passage.position.x + DialogueEntry.CanvasRectWidth / 4f + (linkNum * (DialogueEntry.CanvasRectWidth + 8)), passage.position.y + (1.5f * DialogueEntry.CanvasRectHeight)));
                                linkNum++;
                            }
                            int linkActorID, linkConversantID;
                            string linkDialogueText, sequence, conditions, script, description;
                            ExtractParticipants(link.name, actorID, conversantID, true, out linkDialogueText, out linkActorID, out linkConversantID);
                            ExtractSequenceConditionsScriptDescription(ref linkDialogueText, out sequence, out conditions, out script, out description);
                            linkEntry.DialogueText = ReplaceFormatting(linkDialogueText);
                            linkEntry.ActorID = linkActorID;
                            linkEntry.ConversantID = linkConversantID;
                            linkEntry.Sequence = sequence;
                            linkEntry.conditionsString = AppendCode(linkEntry.conditionsString, conditions);
                            linkEntry.userScript = AppendCode(linkEntry.userScript, script);
                            originEntry.outgoingLinks.Add(new Link(conversation.id, originID, conversation.id, linkEntry.id));
                        }
                        //else
                        {
                            // Otherwise add a link entry between passages:
                            //var linkEntryTitle = GetLinkEntryTitle(link.link, originID);
                            var linkEntry = conversation.GetDialogueEntry(linkEntryTitle);
                            if (linkEntry == null)
                            {
                                linkEntry = template.CreateDialogueEntry(++highestPid, conversation.id, linkEntryTitle);
                                if (useTwineNodePositions)
                                {
                                    SetEntryPosition(linkEntry, new TwinePosition(passage.position.x + DialogueEntry.CanvasRectWidth / 4f + (linkNum * (DialogueEntry.CanvasRectWidth + 8)), passage.position.y + (1.5f * DialogueEntry.CanvasRectHeight)));
                                    linkNum++;
                                }
                                int linkActorID, linkConversantID;
                                string linkDialogueText, sequence, conditions, script, description;
                                ExtractParticipants(link.name, actorID, conversantID, true, out linkDialogueText, out linkActorID, out linkConversantID);
                                ExtractSequenceConditionsScriptDescription(ref linkDialogueText, out sequence, out conditions, out script, out description);
                                linkEntry.DialogueText = ReplaceFormatting(linkDialogueText);
                                linkEntry.ActorID = linkActorID;
                                linkEntry.ConversantID = linkConversantID;
                                linkEntry.Sequence = sequence;
                                linkEntry.conditionsString = AppendCode(linkEntry.conditionsString, conditions);
                                linkEntry.userScript = AppendCode(linkEntry.userScript, script);
                            }
                            conversation.dialogueEntries.Add(linkEntry);
                            if (!willLinkInHook)
                            {
                                originEntry.outgoingLinks.Add(new Link(conversation.id, originID, conversation.id, linkEntry.id));
                            }
                            linkEntry.outgoingLinks.Add(new Link(conversation.id, linkEntry.id, conversation.id, linkedPassageID));
                        }
                    }
                }
            }

            // Link hooks:
            foreach (var passage in story.passages)
            {
                var passageID = SafeConvert.ToInt(passage.pid);
                var passageEntry = conversation.GetDialogueEntry(passageID);
                var rectOffset = Vector2.zero;
                var passageHooks = allHooks[passage];
                foreach (var hook in passageHooks)
                {
                    int hookActorID, hookConversantID;
                    ExtractParticipants(hook.text, passageEntry.ActorID, passageEntry.ConversantID, true, out hook.text, out hookActorID, out hookConversantID);
                    var isIfOrElseif = IfMacroNameRegex.IsMatch(hook.prefix) || ElseIfMacroNameRegex.IsMatch(hook.prefix);
                    var isElse = ElseMacroNameRegex.IsMatch(hook.prefix);
                    var conditions = isIfOrElseif ? ConvertIfMacro(hook.prefix)
                        : isElse ? "(else:)" : string.Empty;
                    if (hook.links.Count == 0)
                    {
                        // Could be conditional link to text or could be (align:):
                        if (!AlignMacroNameRegex.IsMatch(hook.prefix))
                        { 
                            var linkEntryTitle = GetLinkEntryTitle(hook.text, passageID);
                            var linkEntry = conversation.GetDialogueEntry(linkEntryTitle);
                            if (linkEntry != null) linkEntry.conditionsString = conditions;
                        }
                    }
                    else
                    {
                        foreach (var link in hook.links)
                        {
                            var linkEntryTitle = GetLinkEntryTitle(link, passageID);
                            var linkEntry = conversation.GetDialogueEntry(linkEntryTitle);
                            if (linkEntry == null)
                            {
                                Debug.LogWarning($"Twine importer: Can't find link entry '{linkEntryTitle}' for hook prefix='{hook.prefix}' text='{hook.text}'");
                                continue;
                            }
                            if (!string.IsNullOrEmpty(hook.text) && hook.text != linkEntry.DialogueText)
                            {
                                // Hook still has text, so make the text an intermediate entry:
                                var midEntry = template.CreateDialogueEntry(++highestPid, conversation.id, hook.text);
                                ExtractParticipants(hook.text, hookActorID, hookConversantID, true, out var midEntryDialogueText, out var midEntryActorID, out var midEntryConversantID);
                                if (useTwineNodePositions)
                                {
                                    SetEntryPosition(linkEntry, new TwinePosition(passage.position.x + DialogueEntry.CanvasRectWidth / 4f + (linkNum * (DialogueEntry.CanvasRectWidth + 8)), passage.position.y + (1.5f * DialogueEntry.CanvasRectHeight)));
                                    linkNum++;
                                }
                                midEntry.DialogueText = midEntryDialogueText; //hook.text;
                                midEntry.ActorID = hookActorID;
                                midEntry.ConversantID = hookConversantID;

                                string falseConditionAction;
                                CheckConditionsForPassthrough(conditions, out conditions, out falseConditionAction);
                                midEntry.conditionsString = AppendCode(midEntry.conditionsString, conditions);
                                midEntry.falseConditionAction = falseConditionAction;
                                midEntry.outgoingLinks.Add(new Link(conversation.id, midEntry.id, conversation.id, linkEntry.id));

                                conversation.dialogueEntries.Add(midEntry);
                                passageEntry.outgoingLinks.Add(new Link(conversation.id, passageEntry.id, conversation.id, midEntry.id));
                            }
                            else
                            {
                                // Otherwise link directly from passage to link entry:
                                linkEntry.conditionsString = conditions;
                                passageEntry.outgoingLinks.Add(new Link(conversation.id, passageEntry.id, conversation.id, linkEntry.id));
                            }
                        }
                    }
                }
            }

            // Process links specified by (go-to:) macro:
            ProcessGotoLinks();

            // Split pipes:
            if (splitPipesIntoEntries)
            {
                conversation.SplitPipesIntoEntries();
            }

            // For all nodes with (else:) in Conditions, set correct Conditions:
            foreach (var entry in conversation.dialogueEntries)
            {
                foreach (var link in entry.outgoingLinks)
                {
                    var childEntry = conversation.GetDialogueEntry(link.destinationDialogueID);
                    if (childEntry == null) continue;
                    if (childEntry.conditionsString == "(else:)")
                    {
                        childEntry.conditionsString = GetElseConditions(conversation, entry, childEntry);
                    }
                }
            }

            // For all nodes without text, set Sequence to Continue(), and
            // check text for alignment markup such as ==> and 
            // convert DS {markup} to [markup]:
            foreach (var entry in conversation.dialogueEntries)
            {
                var dialogueText = entry.DialogueText;
                if (string.IsNullOrEmpty(dialogueText) &&
                    string.IsNullOrEmpty(entry.Sequence))
                {
                    entry.Sequence = "Continue()";
                }
                else
                {
                    if (ExtractFormattingMarkup(ref dialogueText))
                    {
                        entry.DialogueText = dialogueText;
                    }
                    if (ConvertCurlyBraceDialogueSystemMarkup(ref dialogueText))
                    {
                        entry.DialogueText = dialogueText;
                    }
                }
            }
        }

        protected virtual void SetEntryPosition(DialogueEntry entry, TwinePosition position)
        {
            entry.canvasRect = new Rect(position.x, position.y, DialogueEntry.CanvasRectWidth, DialogueEntry.CanvasRectHeight);
        }

        protected string GetLinkEntryTitle(string linkName, int originPassageID)
        {
            // Include ID to make links with same names unique.
            if (IsWrappedInParens(linkName))
            {
                return linkName;
            }
            return linkName.Replace("[", "").Replace("]", "").Trim() + " from pid " + originPassageID;
        }

        protected virtual void ExtractParticipants(string text, int actorID, int conversantID, bool isLinkEntry,
            out string dialogueText, out int entryActorID, out int entryConversantID)
        {
            ExtractActor(text, actorID, conversantID, isLinkEntry, out dialogueText, out entryActorID);
            if (entryActorID == -1) entryActorID = isLinkEntry ? actorID : conversantID;
            entryConversantID = (entryActorID == actorID) ? conversantID : actorID;
        }

        protected virtual void ExtractActor(string text, int actorID, int conversantID, bool isLinkEntry,
            out string dialogueText, out int entryActorID)
        {
            entryActorID = isLinkEntry ? actorID : conversantID;
            dialogueText = text;
            var colonPos = text.IndexOf(':');
            if (colonPos != -1)
            {
                var potentialActorName = text.Substring(0, colonPos);
                var remainder = text.Substring(colonPos + 1).TrimStart(new char[] { ' ', '\n', '\t' });
                var actor = database.GetActor(potentialActorName);
                if (actor != null)
                {
                    entryActorID = actor.id;
                    dialogueText = remainder;
                }
            }
            dialogueText = dialogueText.Trim();
        }

        protected virtual void ExtractSequenceConditionsScriptDescription(ref string text, out string sequence, out string conditions, out string script, out string description)
        {
            ExtractBlock("Sequence:", ref text, out sequence);
            ExtractBlock("Conditions:", ref text, out conditions);
            ExtractBlock("Script:", ref text, out script);
            ExtractBlock("Description:", ref text, out description);
        }

        protected virtual void ExtractBlock(string heading, ref string text, out string block)
        {
            var index = text.IndexOf(heading);
            if (index != -1)
            {
                var blockIndex = index + heading.Length;
                var sequenceIndex = FindBlockIndex(text, blockIndex, "Sequence:");
                var conditionsIndex = FindBlockIndex(text, blockIndex, "Conditions:");
                var scriptIndex = FindBlockIndex(text, blockIndex, "Script:");
                var descriptionIndex = FindBlockIndex(text, blockIndex, "Description:");
                var rindex = Mathf.Min(sequenceIndex, Mathf.Min(conditionsIndex, scriptIndex));
                block = text.Substring(blockIndex, rindex - blockIndex).Trim();
                var remaining = text.Substring(0, index);
                if (rindex < text.Length) remaining += text.Substring(rindex);
                text = remaining.Trim();
            }
            else
            {
                block = string.Empty;
            }
        }

        protected int FindBlockIndex(string text, int startIndex, string heading)
        {
            var index = text.IndexOf(heading, startIndex);
            return (index == -1) ? text.Length : index;
        }

        protected void CheckConditionsForPassthrough(string originalConditions, out string conditions, out string falseConditionAction)
        {
            var passthrough = false;
            if (!string.IsNullOrEmpty(originalConditions) && originalConditions.StartsWith("(passthrough)"))
            {
                passthrough = true;
                conditions = originalConditions.Substring("(passthrough)".Length);
            }
            else
            {
                conditions = originalConditions;
            }
            falseConditionAction = passthrough ? "Passthrough" : "Block";
        }

        protected string AppendCode(string block, string extra)
        {
            if (string.IsNullOrEmpty(extra)) return block;
            if (string.IsNullOrEmpty(block)) return extra;
            return block + "\n" + extra;
        }

        #endregion

        #region Links and Hooks

        protected const string LinkRegexPattern = @"\[\[.*?\]\]";
        protected static Regex LinkRegex = new Regex(LinkRegexPattern);
        protected static Regex PrefixedHookRegex = new Regex(@"\([^)]*\)\[(?<brackets>(?:\[(?<open>)|\](?<-open>)|[^(])*)(?(open)(?!))\]");
        protected static Regex ConditionalGotoRegex = new Regex(@"\([^)]*\)\[[^\]]*\([\-_]*[Gg][\-_]*[Oo][\-_]*[Tt][\-_]*[Oo][\-_]*:[^\)]+\)[^\]]*\]");

        public class TwineHook
        {
            public string prefix; // (part) before hooks such as (if: x==y).
            public string text; // Remaining text after extracting links.
            public List<string> links;
            public TwineHook(string prefix, string text, List<string> links)
            { this.prefix = prefix; this.text = text; this.links = links; }
        }

        private void ReplaceConditionalGotoWithLinks(TwineStory story)
        {
            foreach (var passage in story.passages)
            {
                passage.text = ConditionalGotoRegex.Replace(passage.text, (match) =>
                {
                    return ReplaceConditionalGotoMatch(story, passage, match.Value);
                });
            }
        }

        private string ReplaceConditionalGotoMatch(TwineStory story, TwinePassage passage, string conditionalGoto)
        {
            // Replace the (goto:) clause with a [[link]]:
            var gotoMatch = GotoMacroNameAnywhereRegex.Match(conditionalGoto);
            var beforeGoto = conditionalGoto.Substring(0, gotoMatch.Index);
            var firstQuotePos = conditionalGoto.IndexOf("\"", gotoMatch.Index);
            var secondQuotePos = conditionalGoto.IndexOf("\"", firstQuotePos + 1);
            var closeParenPos = conditionalGoto.IndexOf(")", secondQuotePos + 1);
            var linkName = conditionalGoto.Substring(firstQuotePos + 1, secondQuotePos - firstQuotePos - 1);
            var gotoClause = conditionalGoto.Substring(gotoMatch.Index, closeParenPos - gotoMatch.Index + 1);
            var text = conditionalGoto.Remove(gotoMatch.Index, gotoClause.Length);
            text = text.Insert(gotoMatch.Index, $"[[{linkName}]]");

            // Add a TwineLink to the passage:
            var links = new List<TwineLink>(passage.links);
            string destinationPid = "";
            foreach (var p in story.passages)
            {
                if (p.name == linkName) destinationPid = p.pid;
            }
            links.Add(new TwineLink() { name = linkName, link = linkName, pid = destinationPid });
            passage.links = links.ToArray();

            return text;
        }

        protected virtual void ExtractHooks(ref string text, out List<TwineHook> hooks)
        {
            hooks = new List<TwineHook>();

            // Extract (...)[...]:
            var matches = PrefixedHookRegex.Matches(text);
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                // If match is inside backticks, it's literal so ignore it:
                if (IsInBacktick(text, match.Index)) continue;

                // Macros followed by links such as "(macro:...)[hook]" aren't hooks,
                // except for if:, else-if:, and else: macros.
                var macroMatch = Regex.Match(match.Value, @"^\(\w+\:");
                if (macroMatch.Success)
                {
                    var isConditionalOrAlignMacro =
                        IfMacroNameRegex.IsMatch(match.Value) ||
                        ElseIfMacroNameRegex.IsMatch(match.Value) ||
                        ElseMacroNameRegex.IsMatch(match.Value) ||
                        AlignMacroNameRegex.IsMatch(match.Value);
                    if (!isConditionalOrAlignMacro) continue;
                }

                // Require format (...) [something].
                var dividerMatch = Regex.Match(match.Value, @"\)(\s)*\[");
                if (!dividerMatch.Success) continue;


                var index = dividerMatch.Index;
                var dividerLength = dividerMatch.Length;

                var prefix = match.Value.Substring(0, index + dividerLength - 1).Trim();
                var hookText = match.Value.Substring(index + dividerLength, match.Length - (prefix.Length + dividerLength)).Trim();
                var containsLink = hookText.Contains("[[") && hookText.Contains("]]");
                List<string> links;
                ExtractLinksFromText(ref hookText, out links);
                hookText = ReplaceFormatting(hookText);
                hooks.Add(new TwineHook(prefix, hookText, links));

                var newlinePos = match.Index + match.Length;
                var hasNewline = 0 <= newlinePos && newlinePos < text.Length && text[newlinePos] == '\n';

                string replacement = string.Empty;
                if (!(containsLink || string.IsNullOrEmpty(hookText)))
                {
                    if (AlignMacroNameRegex.IsMatch(prefix))
                    {
                        replacement = WrapTextInAlignment(StripExteriorQuotes(GetMacroParams(prefix)), hookText);
                    }
                    else
                    {
                        // Note: Conditional() changed -- now expects a bool for first parameter.
                        var condition = ConvertIfMacro(prefix);
                        var cleanHookText = hookText.Replace("\"", "\\\"");
                        if (hasNewline) cleanHookText += "\\n";
                        replacement = "[lua(Conditional(" + condition + ", \"" + cleanHookText + "\"))]";
                    }
                }

                text = Replace(text, match.Index, match.Length + (hasNewline ? 1 : 0), replacement);
            }
        }

        private bool IsInBacktick(string text, int index)
        {
            if (string.IsNullOrEmpty(text) || !(0 <= index && index < text.Length)) return false;
            int numBackticks = 0;
            for (int i = 0; i < index; i++)
            {
                if (text[i] == '`') numBackticks++;
            }
            var isOddNumber = numBackticks % 2 != 0;
            return isOddNumber;
        }

        protected string StripExteriorQuotes(string s)
        {
            if (!string.IsNullOrEmpty(s) && s.StartsWith("\"") && s.EndsWith("\""))
            {
                return s.Substring(1, s.Length - 2);
            }
            else
            {
                return s; 
            }
        }

        protected string WrapTextInAlignment(string alignmentSpecifier, string text)
        {
            switch (alignmentSpecifier)
            {
                default:
                    return text;
                case "<==":
                    return $"<align=\"left\">{text}</align>";
                case "==>":
                    return $"<align=\"right\">{text}</align>";
                case "=><=":
                    return $"<align=\"center\">{text}</align>";
                case "<==>":
                    return $"<align=\"justified\">{text}</align>";
            }
        }

        // Convert alignment markup (e.g., ==>) and heading markup (e.g., #Heading).
        protected virtual bool ExtractFormattingMarkup(ref string text)
        {
            if (!(text.Contains("==") || text.Contains("=>") || text.Contains("#"))) return false;
            var lines = new List<string>(text.Split('\n'));
            var foundHeadingMarkup = false;
            var foundAlignmentToken = false;
            var isInAlignmentBlock = false;
            int safeguard = 0;
            int i = 0;
            while (i < lines.Count && safeguard++ < 999)
            {
                // If line doesn't appear to have an alignment or heading token, skip past it:
                if (!(lines[i].Contains("==") || lines[i].Contains("=>") || lines[i].Contains("#")))
                {
                    i++;
                    continue;
                }
                var line = lines[i].Trim();
                if (line.StartsWith("#"))
                {
                    // Handle heading markup:
                    foundHeadingMarkup = true;
                    int numHashes = CountNumLeadingHashes(line);
                    lines[i] = $"<style=\"H{numHashes}\">{line.Substring(numHashes)}</style>";
                    i++;
                }
                else
                {
                    switch (line)
                    {
                        default:
                            i++; // If no alignment token match, move to next line.
                            break;
                        case "==>":
                            StartAlignmentBlock(lines, i, ref isInAlignmentBlock, ref foundAlignmentToken, "right");
                            break;
                        case "=><=":
                            StartAlignmentBlock(lines, i, ref isInAlignmentBlock, ref foundAlignmentToken, "center");
                            break;
                        case "<==>":
                            StartAlignmentBlock(lines, i, ref isInAlignmentBlock, ref foundAlignmentToken, "justified");
                            break;
                        case "<==":
                            StartAlignmentBlock(lines, i, ref isInAlignmentBlock, ref foundAlignmentToken, "left");
                            break;
                    }
                }
            }
            if (isInAlignmentBlock) lines[lines.Count - 1] += "</align>";
            text = string.Join("\n", lines);
            return foundAlignmentToken || foundHeadingMarkup;
        }

        private int CountNumLeadingHashes(string line)
        {
            int numHashes = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '#')
                {
                    numHashes++; 
                }
                else
                {
                    break;
                }
            }
            return numHashes;
        }

        private void StartAlignmentBlock(List<string> lines, int i, 
            ref bool isInAlignmentBlock, ref bool foundAlignmentToken, 
            string alignment)
        {
            if (isInAlignmentBlock)
            {
                // Close the previous alignment block:
                if (i > 0)
                {
                    lines[i - 1] += $"</align>";
                }
            }
            foundAlignmentToken = true;
            isInAlignmentBlock = true;
            if (i + 1 < lines.Count && alignment != "left") // Left alignment is default, so just close previous block.
            {
                lines[i + 1] = $"<align=\"{alignment}\">{lines[i + 1]}";
            }
            lines.RemoveAt(i);
        }

        protected virtual void ExtractLinksFromText(ref string text, out List<string> links)
        {
            links = new List<string>();

            // Look for links in [[double-brackets]]:
            var matches = LinkRegex.Matches(text);
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                links.Add(match.Value.Substring(2, match.Value.Length - 4));
                text = Replace(text, match.Index, match.Length, string.Empty);
            }

            // Handle link in the form "text -> link":
            var rightArrowPos = text.LastIndexOf("->");
            if (rightArrowPos != -1)
            {
                var link = text.Substring(rightArrowPos + "->".Length).Trim();
                text = text.Substring(0, rightArrowPos);
                links.Add(link);
            }
            else
            {
                // Handle link in the form "text | link":
                var pipePos = text.LastIndexOf("|");
                if (rightArrowPos != -1)
                {
                    var link = text.Substring(pipePos + "|".Length).Trim();
                    text = text.Substring(0, pipePos);
                    links.Add(link);
                }
            }

            text = text.Trim();
        }

        protected virtual string RemoveAllLinksFromText(string text)
        {
            text = LinkRegex.Replace(text, (match) =>
            {
                return IsInBacktick(text, match.Index) ? match.Value : string.Empty;
            });
            return text;
        }

        protected bool IsLinkInHooks(string link, List<TwineHook> hooks)
        {
            foreach (var hook in hooks)
            {
                if (hook.links.Contains(link)) return true;
            }
            return false;
        }

        protected bool IsLinkImplicit(TwineLink link)
        {
            if (link.name == null) return true;
            return IsWrappedInParens(link.name);
        }

        protected bool IsWrappedInParens(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 3) return false;
            return name[0] == '(' && name[name.Length - 1] == ')';

        }

        protected string RemoveParensFromPassageName(string passageName)
        {
            return IsWrappedInParens(passageName)
                ? passageName.Substring(1, passageName.Length - 2)
                : passageName;
        }

        #endregion

        #region Formatting

        protected virtual string RemoveFormatting(string s)
        {
            var result = Regex.Replace(s, @"\/\/.*?\/\/|\'\'.*?\'\'|\*\*.*?\*\*|\*.*?\*", string.Empty);
            result = Regex.Replace(result, @"==>|=><=|<==", string.Empty);
            return result.Trim();
        }

        protected virtual string ReplaceFormatting(string s)
        {
            // Replace formatting codes:
            s = ReplaceFormattingCode(s, "//", "<i>", "</i>");
            s = ReplaceFormattingCode(s, "''", "<b>", "</b>");
            s = ReplaceFormattingCode(s, "**", "<b>", "</b>");
            s = ReplaceFormattingCode(s, "*", "<i>", "</i>");
            s = ReplaceFormattingCode(s, "~~", "<s>", "</s>"); // TMPro
            s = ReplaceFormattingCode(s, "^^", "<sup>", "</sup>"); // TMPro

            // Replace variables:
            s = ReplaceVariables(s);

            return s;
        }

        protected virtual string ReplaceFormattingCode(string s, string formatCode, string richCodeOpen, string richCodeClose)
        {
            int safeguard = 0;
            while (s.Contains(formatCode) && safeguard++ < 999)
            {
                var index = s.IndexOf(formatCode);
                var nextIndex = (index + formatCode.Length < s.Length) ? s.IndexOf(formatCode, index + 2) : -1;
                if (nextIndex == -1) break; // Not paired, so stop.
                s = s.Substring(0, index) + richCodeOpen +
                    s.Substring(index + formatCode.Length, nextIndex - (index + formatCode.Length)) + richCodeClose +
                    s.Substring(nextIndex + formatCode.Length);
            }
            return s;
        }

        protected static Regex GlobalVariableRegex = new Regex(@"\$\w+");
        protected static Regex LocalVariableRegex = new Regex(@"_\w+");

        protected virtual string ReplaceVariables(string s)
        {
            // Replace $x with Variable["x"]:
            var matches = GlobalVariableRegex.Matches(s);
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                var varTag = "[var=" + match.Value.Substring(1) + "]";
                s = Replace(s, match.Index, match.Length, varTag);
            }

            // Replace _x with [lua(_x)]:
            matches = LocalVariableRegex.Matches(s);
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                var varTag = "[lua(" + match.Value.Substring(1) + ")]";
                s = Replace(s, match.Index, match.Length, varTag);
            }

            return s;
        }

        protected string Replace(string s, int index, int length, string replacement)
        {
            var builder = new System.Text.StringBuilder();
            builder.Append(s.Substring(0, index));
            builder.Append(replacement);
            builder.Append(s.Substring(index + length));
            return builder.ToString();
        }

        #endregion

        #region Macros

        // Macros are case-insensitive and can have any - or _ inside them.

        protected static Regex MacroRegex = new Regex(@"\([\w\-_]+:(?:[^()]+|\((?<Depth>)|\)(?<-Depth>))*(?(Depth)(?!))\)");
        protected static Regex IfMacroNameRegex = new Regex(@"^\([\-_]*[Ii][\-_]*[Ff][\-_]*:");
        protected static Regex ElseIfMacroNameRegex = new Regex(@"^\([\-_]*[Ee][\-_]*[Ll][\-_]*[Ss][\-_]*[Ee][\-_]*[Ii][\-_]*[Ff][\-_]*:");
        protected static Regex ElseMacroNameRegex = new Regex(@"^\([\-_]*[Ee][\-_]*[Ll][\-_]*[Ss][\-_]*[Ee][\-_]*:");
        protected static Regex SetMacroNameRegex = new Regex(@"^\([\-_]*[Ss][\-_]*[Ee][\-_]*[Tt][\-_]*:");
        protected static Regex GotoMacroNameRegex = new Regex(@"^\([\-_]*[Gg][\-_]*[Oo][\-_]*[Tt][\-_]*[Oo][\-_]*:");
        protected static Regex GotoMacroNameAnywhereRegex = new Regex(@"\([\-_]*[Gg][\-_]*[Oo][\-_]*[Tt][\-_]*[Oo][\-_]*:");
        protected static Regex PrintMacroNameRegex = new Regex(@"^\([\-_]*[Pp][\-_]*[Rr][\-_]*[Ii][\-_]*[Nn][\-_]*[Tt][\-_]*:");
        protected static Regex AlignMacroNameRegex = new Regex(@"^\([\-_]*[Aa][\-_]*[Ll][\-_]*[Ii][\-_]*[Gg][\-_]*[Nn][\-_]*:");
        protected static Regex CondMacroNameRegex = new Regex(@"^\([\-_]*[Cc][\-_]*[Oo][\-_]*[Nn][\-_]*[Dd][\-_]*:");

        protected void ExtractMacros(ref string s, ref DialogueEntry entry)
        {
            var matches = MacroRegex.Matches(s);
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                if (IsInBacktick(s, match.Index)) continue;
                entry.userScript = AppendCode(ConvertMacro(match.Value, entry, out var replacement), entry.userScript);
                s = Replace(s, match.Index, match.Length, replacement);
            }
            s.Trim();
        }

        protected string ConvertMacro(string macro, DialogueEntry entry, out string replacement)
        {
            replacement = string.Empty;
            if (string.IsNullOrEmpty(macro)) return macro;
            if (SetMacroNameRegex.IsMatch(macro))
            {
                return ConvertSetMacro(macro);
            }
            else if (GotoMacroNameRegex.IsMatch(macro))
            {
                return ConvertGoToMacro(macro, entry);
            }
            else if (PrintMacroNameRegex.IsMatch(macro))
            {
                return ConvertPrintMacro(macro);
            }
            else if (CondMacroNameRegex.IsMatch(macro))
            {
                return ConvertCondMacro(macro, entry, out replacement);
            }
            else
            {
                Debug.LogWarning("This Twine macro is not supported yet: " + macro);
                return "UnhandledTwineMacro(" + macro + ")";
            }
        }

        // Strip "(macro:" from beginning and ")" from end:
        protected string GetMacroParams(string macro)
        {
            var s = macro.Trim();
            var colonPos = s.IndexOf(':');
            s = s.Substring(colonPos + 1, s.Length - (colonPos + 2)).Trim();
            return s;
        }

        protected string ConvertSetMacro(string macro)
        {
            var s = GetMacroParams(macro);
            var tokens = s.Split(' ');
            if (tokens.Length < 3) return macro;
            var lua = string.Empty;
            var startNewExpression = true;
            for (int i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                if (token == "to") token = "=";
                if (startNewExpression)
                {
                    if (!string.IsNullOrEmpty(lua)) lua += ";\n";
                    lua += ConvertVariableToLua(token);
                    startNewExpression = false;
                }
                else
                {
                    if (token.EndsWith(","))
                    {
                        token = token.Substring(0, token.Length - 1);
                        startNewExpression = true;
                    }
                    lua += " " + ConvertVariableToLua(token);
                }
            }
            return lua;
        }

        protected string ConvertPrintMacro(string macro)
        {
            var s = GetMacroParams(macro);
            s = ConvertVariablesToLua(s);
            return $"print({s})";
        }

        // Syntax: (cond: condition1, value1, condition2, value2, ..., conditionN, valueN, failValue)
        protected string ConvertCondMacro(string macro, DialogueEntry entry, out string replacement)
        {
            replacement = string.Empty;
            var fields = GetMacroParams(macro).Split(',');
            var cumulativeCondition = string.Empty;
            int i = 0;
            int safeguard = 0;
            while (i < fields.Length - 1 && safeguard++ < 999)
            {
                var condition = fields[i++].Trim();
                var value = fields[i++].Trim();
                condition = ConvertTwineCodeToLua(condition, removeFirstToken: false);
                var lua = $"[lua(Conditional({condition}, {value}))]";
                replacement += lua;
                if (!string.IsNullOrEmpty(cumulativeCondition)) cumulativeCondition += " or ";
                cumulativeCondition += condition;
            }
            //[TODO]: Fix Conditional() to allow this:
            //// Add fail value:
            //if (i == fields.Length - 1)
            //{
            //    replacement += $"[lua(Conditional(not ({cumulativeCondition}), {fields[fields.Length - 1]}))]";
            //}
            return string.Empty;
        }

        protected string ConvertGoToMacro(string macro, DialogueEntry entry)
        {
            var s = GetMacroParams(macro);
            s = s.Replace("\"", "");
            var gotoLink = new GotoLink();
            gotoLink.entry = entry;
            gotoLink.destinationTitle = s;
            gotoLinks.Add(gotoLink); // Need to wait until all entries are created.
            return string.Empty;
        }

        protected void ProcessGotoLinks()
        {
            foreach (var gotoLink in gotoLinks)
            {
                var destinationEntry = currentConversation.dialogueEntries.Find(x => x.Title == gotoLink.destinationTitle);
                if (destinationEntry == null)
                {
                    Debug.LogWarning($"Can't connect \"{gotoLink.entry.Title}\" to dialogue entry titled \"{gotoLink.destinationTitle}\"");
                }
                else
                {
                    gotoLink.entry.outgoingLinks.Add(new Link(gotoLink.entry.conversationID, gotoLink.entry.id, destinationEntry.conversationID, destinationEntry.id));
                }
            }
        }

        protected string ConvertIfMacro(string macro)
        {
            var s = macro.Trim();
            s = s.Substring(0, s.Length - 1); // Remove last paren.

            // Insert space in case there's no space between if: and condition:
            var colonPos = s.IndexOf(':');
            if (colonPos != -1 && !s.Contains(": ")) s = s.Substring(0, colonPos + 1) + ' ' + s.Substring(colonPos + 1);

            var lua = ConvertTwineCodeToLua(s, removeFirstToken: true);
            return lua;
        }

        protected virtual string ConvertTwineCodeToLua(string s, bool removeFirstToken)
        {
            var tokens = new List<string>(s.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries));
            if (removeFirstToken) tokens.RemoveAt(0); // Remove (if: or (else-if:.
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "is") tokens[i] = "==";
                else tokens[i] = ConvertVariableToLua(tokens[i]);
            }
            var lua = string.Join(" ", tokens);
            return lua;
        }

        protected string ConvertVariableToLua(string variable)
        {
            if (variable.StartsWith("$"))
            {
                var variableNameWithoutDollar = variable.Substring(1);
                AddVariableToDatabase(variableNameWithoutDollar);
                return "Variable[\"" + variableNameWithoutDollar + "\"]";
            }
            else if (variable.StartsWith("_"))
            {
                return variable.Substring(1);
            }
            else
            {
                return variable;
            }
        }

        // Convert all instances of $varname to Variable["varname"]:
        protected string ConvertVariablesToLua(string s)
        {
            return Regex.Replace(s, @"\$\w+\b", (match) =>
            {
                return "Variable[\"" + match.Value.Substring(1) + "\"]";
            });
        }

        protected void AddVariableToDatabase(string variableName)
        {
            if (database.GetVariable(variableName) == null)
            {
                database.variables.Add(template.CreateVariable(template.GetNextVariableID(database), variableName, string.Empty));
            }
        }

        protected string GetElseConditions(Conversation conversation, DialogueEntry parentEntry, DialogueEntry childEntry)
        {
            if (conversation == null || parentEntry == null || childEntry == null) return string.Empty;
            var first = true;
            var result = "not (";
            foreach (var link in parentEntry.outgoingLinks)
            {
                var siblingEntry = conversation.GetDialogueEntry(link.destinationDialogueID);
                if (siblingEntry == null) continue;
                if (siblingEntry == childEntry) continue;
                if (string.IsNullOrEmpty(siblingEntry.conditionsString)) continue;
                if (siblingEntry.conditionsString == "(else:)") continue;
                if (!first) result += " or ";
                first = false;
                result += $"({siblingEntry.conditionsString})";
            }
            result += ")";
            return result;
        }

        #endregion

        #region Curly Brace Conversion

        protected static Regex CurlyBraceMarkupRegex = new Regex(
            @"{f}|{auto}|{a}|{nosubtitle}|{em[\d]+}|{/em\d+}|" +
            @"{var=\w+}|{var=?w+}|{autocase=\w+}|{pic=\d+}|{pica=\d+}|{picc=\d+}|" +
            @"{position=\d+}|{panel=\d+}");
        // Note: Does not handle [lua(code)].

        // Convert {dsmarkup} to [dsmarkup]:
        protected virtual bool ConvertCurlyBraceDialogueSystemMarkup(ref string text)
        {
            if (!text.Contains("{")) return false;
            var replacedCurlyBraceMarkup = false;
            text = CurlyBraceMarkupRegex.Replace(text, (match) =>
            {
                replacedCurlyBraceMarkup = true;
                return "[" + match.Value.Substring(1, match.Value.Length - 2) + "]";
            });
            return replacedCurlyBraceMarkup;
        }

        #endregion

    }
}
#endif
