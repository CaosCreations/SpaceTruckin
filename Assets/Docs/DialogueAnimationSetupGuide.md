# Dialogue Animation Setup Guide

This guide walks through how to set up and integrate dialogue animation assets in the Unity editor. It covers two systems that work together during conversations:

1. **Intro Animations** - A full-screen sprite animation that plays before a conversation starts (like a character's entrance).
2. **Composite Portraits** - Layered face portraits with swappable eyes and mouths that change expression during dialogue, with automatic blinking.

There is also a legacy **Single-Portrait Expression** system that uses pre-made full portraits instead of layered parts. Both portrait systems are supported and the game will fall back to single portraits automatically if composite data isn't available.

---

## Table of Contents

- [Part 1: Intro Animations](#part-1-intro-animations)
  - [Preparing Your Sprite Frames](#preparing-your-sprite-frames)
  - [Creating the Animation Data Asset](#creating-the-animation-data-asset)
  - [Registering the Animation](#registering-the-animation)
  - [Linking the Animation to an Actor](#linking-the-animation-to-an-actor)
  - [Setting Up the Scene Trigger](#setting-up-the-scene-trigger)
- [Part 2: Composite Portraits (Layered Faces)](#part-2-composite-portraits-layered-faces)
  - [Preparing Your Face Art](#preparing-your-face-art)
  - [Creating the Face Data Asset](#creating-the-face-data-asset)
  - [Registering the Face Data](#registering-the-face-data)
  - [Setting Expressions on Dialogue Nodes](#setting-expressions-on-dialogue-nodes)
  - [How Expression Persistence Works](#how-expression-persistence-works)
- [Part 3: Single-Portrait Expressions (Legacy/Fallback)](#part-3-single-portrait-expressions-legacyfallback)
  - [Assigning Portraits in the Dialogue Database](#assigning-portraits-in-the-dialogue-database)
  - [Using the Facial Expression Preview Window](#using-the-facial-expression-preview-window)
  - [Setting Expressions on Dialogue Nodes (Legacy)](#setting-expressions-on-dialogue-nodes-legacy)
- [Part 4: Testing and Debugging](#part-4-testing-and-debugging)
- [Quick Reference](#quick-reference)
- [Troubleshooting](#troubleshooting)

---

## Part 1: Intro Animations

Intro animations play a short full-screen sprite sequence before a conversation begins. Think of it like a quick cutscene that introduces the character before they start talking.

### Preparing Your Sprite Frames

1. Create a sequence of sprites that form the animation (like frames of a flipbook).
2. Import them into Unity under a folder like `Assets/Art/IntroAnimations/CharacterName/`.
3. Make sure each sprite is set to **Sprite (2D and UI)** texture type in the import settings.
4. Name them in order (e.g., `frame_01`, `frame_02`, ...) so they're easy to drag in sequence.

### Creating the Animation Data Asset

1. In the **Project** window, right-click in the folder where you want to store animation data.
2. Go to **Create > ScriptableObjects > DialogueIntroAnimations > ActorIntroAnimationData**.
3. A new asset file will appear. Name it something clear like `Stefano_Intro`.
4. Select the new asset and fill in the **Inspector**:

| Field | What to Enter |
|-------|--------------|
| **Key** | A unique name that identifies this animation (e.g., `Stefano_Intro`). This must match what you enter in the Dialogue Database later. |
| **Frames** | Drag all your sprite frames here **in order**. The size number shows how many frames are loaded. |
| **Frame Rate** | Time in seconds between each frame. Default is `0.1` (10 frames per second). Lower = faster animation, higher = slower. |

### Registering the Animation

The game uses a **registry** to look up animations by their key. You need to add your new animation data to a registry asset.

1. Find or create an **ActorIntroAnimationRegistry** asset:
   - To create one: right-click > **Create > ScriptableObjects > DialogueIntroAnimations > ActorIntroAnimationRegistry**.
2. Select the registry asset in the Inspector.
3. In the **Elements** array, increase the size by 1 and drag your new `ActorIntroAnimationData` asset into the new slot.

### Linking the Animation to an Actor

The system knows which animation to play by reading a custom field from the actor in the **Dialogue Database**. This field doesn't exist by default - you need to add it first.

**Adding the custom field to the database template (so it appears on all actors):**

1. Open the **Dialogue Editor** window (menu: **Tools > Pixel Crushers > Dialogue Editor**).
2. Click the **Templates** tab (or expand the **Templates** foldout at the bottom of the Database tab).
3. Under **Actor Fields**, click the **+** button to add a new row.
4. Set the **Title** to `IntroAnimationKey` and the **Type** to `Text`.
5. Click **Apply Template To Assets** to push this new field to all existing actors. New actors will get it automatically.

**Alternatively, add the field to a single actor only:**

1. In the Dialogue Editor, go to the **Actors** tab.
2. Select the actor.
3. Scroll to the bottom of their fields list and click the **+** (or **All Fields**) button to add a new field.
4. Set the title to `IntroAnimationKey`, type to `Text`.

**Setting the value:**

1. Go to the **Actors** tab and select the actor who should have an intro animation.
2. Find the **IntroAnimationKey** field (it should now be visible).
3. Set its value to the **Key** you entered on the animation data asset (e.g., `Stefano_Intro`).

### Setting Up the Scene Trigger

The intro animation is triggered by a special dialogue trigger component in the scene.

1. Find the GameObject that starts the conversation (the one with the dialogue trigger).
2. Replace (or use) the **Intro Animation Dialogue System Trigger** component instead of the regular `DialogueSystemTrigger`.
   - You can find it in the **Add Component** menu under: **Pixel Crushers > Dialogue System > Trigger > Intro Animation Dialogue System Trigger**.
3. Fill in the Inspector fields:

| Field | What to Assign |
|-------|---------------|
| **Animation Registry** | Drag in the `ActorIntroAnimationRegistry` asset that contains your animation. |
| **Animation Player** | Drag in the `ActorIntroAnimationPlayer` component from your scene (usually on a UI Canvas). |
| **Conversation** | Set the conversation as you normally would for any dialogue trigger. |

4. The **ActorIntroAnimationPlayer** component should already exist in the scene on a UI Canvas. It needs:
   - A reference to its **Canvas** (the overlay canvas that displays the animation).
   - A reference to an **Image** component (the UI Image that shows each frame).

**What happens at runtime:** When the player triggers the conversation, the system looks up the conversant actor's `IntroAnimationKey`, finds the matching animation in the registry, plays it full-screen, and then automatically starts the conversation when the animation finishes. If no animation key is found, the conversation starts immediately as normal.

---

## Part 2: Composite Portraits (Layered Faces)

Composite portraits layer three images on top of each other - a **base face**, **eyes**, and **mouth** - so you can mix and match expressions without drawing every combination. The system also adds automatic blinking.

### Preparing Your Face Art

You need to create separate sprite layers for each character:

| Layer | Description | Quantity |
|-------|------------|----------|
| **Base Sprite** | The character's face without eyes or mouth (or with a neutral placeholder). | 1 |
| **Eyes** | Different eye states (e.g., neutral, happy, angry, sad). All eyes must be the same size and positioned to overlay correctly on the base. | 2+ |
| **Mouths** | Different mouth states (e.g., closed, smile, frown, open). Same size/position rules as eyes. | 2+ |
| **Blink Sprites** (optional) | Eyes-closed versions. You can make one **universal blink** that works with all eye states, or make **per-eye blinks** that match each eye variant. | 1 or match eye count |

All sprites should be the same dimensions and align when stacked.

### Creating the Face Data Asset

1. Right-click in the Project window > **Create > ScriptableObjects > DialogueIntroAnimations > ActorFaceData**.
2. Name it after the character (e.g., `Stefano_FaceData`).
3. Fill in the Inspector:

| Field | What to Enter |
|-------|--------------|
| **Key** | The actor's **exact name** as it appears in the Dialogue Database (e.g., `Stefano`). |
| **Base Sprite** | Drag in the base face layer. |
| **Eyes** | Drag in eye sprites in order. Index 0 = first eye state, Index 1 = second, etc. |
| **Mouths** | Drag in mouth sprites in order. Index 0 = first mouth state, Index 1 = second, etc. |
| **Universal Blink Sprite** | (Optional) A single blink sprite used for all eye states. |
| **Eye Blinks** | (Optional) Per-eye blink sprites. If provided, each slot corresponds to the eye at the same index. Falls back to Universal Blink if a slot is empty. |
| **Blink Interval Min** | Minimum seconds between blinks. Default: `2`. |
| **Blink Interval Max** | Maximum seconds between blinks. Default: `6`. |
| **Blink Duration** | How long each blink lasts in seconds. Default: `0.15`. |

### Registering the Face Data

1. Find or create an **ActorFaceDataRegistry** asset:
   - Right-click > **Create > ScriptableObjects > DialogueIntroAnimations > ActorFaceDataRegistry**.
2. Add your new `ActorFaceData` asset to its **Elements** array.
3. Make sure the **FacialExpressionSubtitlePanel** in your dialogue UI has a reference to this registry (the `Face Data Registry` field in the Inspector).

### Setting Expressions on Dialogue Nodes

In the Dialogue Editor, each dialogue node (line of text) can have an **`Expression`** field that controls the speaker's face.

The format is: **`E<eye>M<mouth>`**

- `E` followed by the **eye index** (0 or 1)
- `M` followed by the **mouth index** (0 or 1)

**Examples:**

| Expression Value | Meaning |
|-----------------|---------|
| `E0M0` | Eyes index 0, Mouth index 0 |
| `E1M0` | Eyes index 1, Mouth index 0 |
| `E0M1` | Eyes index 0, Mouth index 1 |
| `E1M1` | Eyes index 1, Mouth index 1 |

**Adding the Expression field to the template (so it appears on all dialogue nodes):**

1. In the Dialogue Editor, open the **Templates** tab.
2. Under **Dialogue Entry Fields**, click **+** to add a new row.
3. Set the **Title** to `Expression` and the **Type** to `Text`.
4. Click **Apply Template To Assets** to add it to all existing nodes.

**Setting the value on a dialogue node:**

1. Open the **Dialogue Editor** and select a conversation.
2. Click on a dialogue node.
3. Find the **Expression** field (visible after adding it to the template, or click **All Fields** on the node).
4. Enter the value using the format above.

### How Expression Persistence Works

You don't need to set an expression on every single dialogue node. The system remembers the last expression set for each character and keeps using it until a new one is specified.

- If a node has an `Expression` field, it updates the character's face.
- If a node has **no** `Expression` field (or it's blank), the character keeps whatever expression they had on their previous line.
- When a new conversation starts, all persisted expressions are cleared and characters default to `E0M0`.

---

## Part 3: Single-Portrait Expressions (Legacy/Fallback)

This older system uses pre-drawn full portrait images for each expression. It's still fully supported and is used automatically when a character doesn't have composite face data set up.

### Assigning Portraits in the Dialogue Database

Each actor in the Dialogue Database can have up to 9 portrait images:

| Index | Slot | Use |
|-------|------|-----|
| **0** | Default Portrait | The character's normal/default appearance. |
| **1-8** | Alternate Portraits | Different expressions (e.g., 1=happy, 2=sad, 3=angry, etc.). |

You can assign these in the Dialogue Editor's **Actors** tab, or use the custom preview window (see below).

### Using the Facial Expression Preview Window

A custom editor tool lets you see and assign all 9 portrait slots visually:

1. Open it from the menu: **Space Truckin > Dialogue > Facial Expression Preview**.
2. Select your **Dialogue Database** at the top.
3. Pick an **Actor** from the dropdown.
4. You'll see a 3x3 grid showing all 9 portrait slots.
5. Click **Edit Mode** to drag sprites into the slots.
6. Click **Show All** to see actors that don't have any portraits assigned yet.

Changes made in Edit Mode are saved to the database automatically.

### Setting Expressions on Dialogue Nodes (Legacy)

For single-portrait expressions, use the **`FacialExpression`** field (not `Expression`) on dialogue nodes.

**Adding the field to the template (if you don't already see it on nodes):**

1. In the Dialogue Editor, open the **Templates** tab.
2. Under **Dialogue Entry Fields**, click **+** to add a new row.
3. Set the **Title** to `FacialExpression` and the **Type** to `Text`.
4. Click **Apply Template To Assets**.

**Setting the value:**

1. In the Dialogue Editor, select a dialogue node.
2. Find the **FacialExpression** field.
3. Enter a number **0-8** matching the portrait index you want to show.

If no `FacialExpression` field is set, the default portrait (index 0) is used.

---

## Part 4: Testing and Debugging

### Facial Expression Tester (Runtime)

A runtime testing component lets you preview expressions during Play mode:

1. Add the **FacialExpressionTester** component to any GameObject with an Image.
2. In the Inspector, enter the **Actor Name** (must match the Dialogue Database).
3. Use the numbered buttons (0-8) to switch between expressions instantly.
4. Use **Cycle Expression** to step through them one by one.
5. Enable **Auto Refresh** to see changes immediately when you adjust the slider.

### Common Debugging Steps

- **Animation doesn't play:** Check that the actor's `IntroAnimationKey` field matches the `Key` on the animation data asset, and that the asset is in the registry.
- **Wrong expression showing:** Verify the `Expression` field format is exactly `E0M0` (no spaces, correct capitalization).
- **Blinking not working:** Make sure either `Universal Blink Sprite` or the matching `Eye Blinks` slot is assigned on the face data.
- **Portrait not showing at all:** Confirm the `ActorFaceDataRegistry` is assigned on the `FacialExpressionSubtitlePanel` component in the dialogue UI.

---

## Quick Reference

### File Locations

| What | Where to Create | Menu Path |
|------|----------------|-----------|
| Intro Animation Data | Any asset folder | Create > ScriptableObjects > DialogueIntroAnimations > ActorIntroAnimationData |
| Intro Animation Registry | Any asset folder | Create > ScriptableObjects > DialogueIntroAnimations > ActorIntroAnimationRegistry |
| Face Data | Any asset folder | Create > ScriptableObjects > DialogueIntroAnimations > ActorFaceData |
| Face Data Registry | Any asset folder | Create > ScriptableObjects > DialogueIntroAnimations > ActorFaceDataRegistry |

### Dialogue Database Custom Fields

| Field Name | Where It Goes | Value Format | Purpose |
|-----------|--------------|-------------|---------|
| `IntroAnimationKey` | Actor | Text (e.g., `Stefano_Intro`) | Links actor to their intro animation |
| `Expression` | Dialogue Node | `E<0-1>M<0-1>` (e.g., `E0M1`) | Sets composite portrait expression |
| `FacialExpression` | Dialogue Node | Number `0-8` | Sets single-portrait expression (legacy) |

### Scene Components

| Component | Purpose | Key Fields |
|-----------|---------|------------|
| `IntroAnimationDialogueSystemTrigger` | Plays intro animation before conversation | Animation Registry, Animation Player, Conversation |
| `ActorIntroAnimationPlayer` | Plays sprite frame sequences on a UI canvas | Canvas, Image |
| `FacialExpressionSubtitlePanel` | Dialogue subtitle panel with expression support | Composite Portrait, Face Data Registry |
| `CompositePortraitRenderer` | Renders layered base/eyes/mouth portraits | Root, Base Image, Mouth Image, Eyes Image |

---

## Troubleshooting

**Q: I added frames but the animation is too fast/slow.**
A: Adjust the **Frame Rate** field on the animation data asset. The value is in seconds per frame: `0.05` = very fast, `0.1` = moderate, `0.2` = slow.

**Q: The conversation starts without playing the intro animation.**
A: Make sure you're using `IntroAnimationDialogueSystemTrigger` (not the regular `DialogueSystemTrigger`), and that both the registry and player references are assigned.

**Q: My character blinks too often / not enough.**
A: Adjust `Blink Interval Min` and `Blink Interval Max` on their `ActorFaceData` asset. Higher values = less frequent blinking.

**Q: Expressions aren't changing between dialogue lines.**
A: The `Expression` field must be set on each node where you want the expression to change. Blank fields intentionally keep the previous expression.

**Q: Can I use both composite and single-portrait systems for different characters?**
A: Yes. Characters with face data in the registry will use composite portraits. Characters without face data will automatically fall back to single-portrait mode.
