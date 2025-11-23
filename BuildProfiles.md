# Build Profiles: Dev / Test / Prod

This document explains how to use **Scripting Define Symbols** in Unity to control
dev/test-only debug features (like the `GameTimeHUD`) so they **never** appear in
production builds.

The key symbols used:

- `DEV_BUILD`  → For development builds
- `TEST_BUILD` → For test/QA builds
- *(no symbol)* → For production builds

Scripts use these preprocessor checks:

```csharp
#if UNITY_EDITOR || DEV_BUILD || TEST_BUILD
// debug-only code (e.g., GameTimeHUD)
#endif

So:

Editor: debug HUD is always visible (UNITY_EDITOR).

Dev/Test builds: HUD visible (symbol defined).

Prod builds: HUD hidden/disabled (no symbol).



-------------
1. Dev Build Checklist
Use this when building from a dev branch.

Checkout dev branch

In Git (or GitHub Desktop), ensure you are on your dev branch (for example dev).

Open Scripting Define Symbols

In Unity:
Edit → Project Settings… → Player

Select platform tab (e.g. PC, Mac & Linux Standalone).

Expand Other Settings.

Find Scripting Define Symbols.

Set DEV_BUILD

If the field is empty, set:

text
Copy code
DEV_BUILD
If there are existing symbols, append:

text
Copy code
;DEV_BUILD
Build the dev version

Go to File → Build Settings….

Make sure the correct scenes are in the Build.

Click Build (or Build and Run) and save to a Builds/Dev folder, or similar.

Quick sanity check

Run the build you just created.

Confirm:

The game runs normally.

The tiny GameTimeHUD (HH:MM in the corner) is visible.

In the Unity Editor, the HUD is also visible because UNITY_EDITOR is always defined.

2. Test Build Checklist
Use this when building from a test/QA branch.

Checkout test branch

In Git, ensure you are on your test/QA branch (for example test or release-candidate).

Open Scripting Define Symbols

Edit → Project Settings… → Player

Select PC, Mac & Linux Standalone (or your target platform).

Expand Other Settings.

Find Scripting Define Symbols.

Set TEST_BUILD

Remove DEV_BUILD if present.

If the field is empty, set:

text
Copy code
TEST_BUILD
If there are existing symbols, append:

text
Copy code
;TEST_BUILD
Build the test version

File → Build Settings…

Verify scenes and settings.

Click Build and save to a Builds/Test folder (or similar).

Quick sanity check

Run the test build.

Confirm:

The game runs normally.

The GameTimeHUD appears (HH:MM) as a small debug overlay.

In the Unity Editor, the HUD is also visible (because of UNITY_EDITOR).

If you ever need different behavior for dev vs. test, you can branch on:

csharp
Copy code
#if DEV_BUILD
    // dev-specific behavior
#elif TEST_BUILD
    // test-specific behavior
#endif
3. Production Build Checklist
Use this when building from your main / production branch.

Checkout main/prod branch

In Git, ensure you are on your main/prod branch (for example main or master).

Open Scripting Define Symbols

Edit → Project Settings… → Player

Select your target platform tab.

Expand Other Settings.

Find Scripting Define Symbols.

Remove dev/test symbols

Delete any DEV_BUILD or TEST_BUILD entries.

Keep any unrelated symbols you need.

Final value should not contain DEV_BUILD or TEST_BUILD.

Build the production version

File → Build Settings…

Confirm scenes & settings are correct.

Click Build and save to a Builds/Prod folder (or similar).

Production sanity check

Run the production build.

Confirm:

The game runs normally.

The GameTimeHUD (HH:MM label) does not appear.

Important: In the Unity Editor, the HUD will still be visible because of UNITY_EDITOR.
To verify true prod behavior, always check the standalone build.

4. Quick Reference: Preprocessor Conditions
The debug HUD and other dev-only features are guarded like this:

csharp
Copy code
#if UNITY_EDITOR || DEV_BUILD || TEST_BUILD
    // Code runs in:
    // - Unity Editor
    // - Dev builds (DEV_BUILD defined)
    // - Test builds (TEST_BUILD defined)
#else
    // Code runs in:
    // - Production builds only (no DEV_BUILD / TEST_BUILD)
#endif
Typical usage in the HUD:

csharp
Copy code
#if !(UNITY_EDITOR || DEV_BUILD || TEST_BUILD)
    // In production builds: hide label & disable component.
    timeLabel.enabled = false;
    enabled = false;
#endif