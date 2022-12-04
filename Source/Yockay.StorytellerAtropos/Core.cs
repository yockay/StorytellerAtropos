using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Yockay.StorytellerAtropos
{
  public class Core : Mod
  {
    public static AtroposModSettings settings;

    public override string SettingsCategory() => "Storyteller Atropos";

    public override void DoSettingsWindowContents(Rect inRect) => settings.DoSettingsWindowContents(inRect);

    public Core(ModContentPack content) : base(content)
    {
      settings = GetSettings<AtroposModSettings>();
    }
  }
}
