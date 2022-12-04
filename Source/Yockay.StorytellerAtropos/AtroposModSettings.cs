using RimWorld;
using UnityEngine;
using Verse;

namespace Yockay.StorytellerAtropos
{
  public class AtroposModSettings : ModSettings
  {
    public float minDaysPassed = ConstValue.DefaultMinDaysPassed;
    public float cycleDays = ConstValue.DefaultCycleDays;
    public float announceBeforeDays = ConstValue.DefaultAnnounceBeforeDays;
    public bool debugMode = false;

    public override void ExposeData()
    {
      base.ExposeData();

      Scribe_Values.Look(ref this.minDaysPassed, "minDaysPassed", ConstValue.DefaultMinDaysPassed);
      Scribe_Values.Look(ref this.cycleDays, "cycleDays", ConstValue.DefaultCycleDays);
      Scribe_Values.Look(ref this.announceBeforeDays, "announceBeforeDays", ConstValue.DefaultAnnounceBeforeDays);
      Scribe_Values.Look(ref this.debugMode, "debugMode", false);
    }

    public void DoSettingsWindowContents(Rect inRect)
    {
      Listing_Standard listing = new Listing_Standard();
      listing.Begin(inRect);

      string buffer = null;
      listing.Label("Yockay_Atropos.MinDaysPassedExplain1".Translate());
      listing.Label("Yockay_Atropos.MinDaysPassedExplain2".Translate());
      listing.TextFieldNumericLabeled(
        $"{"Yockay_Atropos.MinDaysPassed".Translate()}  ",
        ref this.minDaysPassed,
        ref buffer,
        0.1f,
        10000f
      );
      listing.Gap();

      buffer = null;
      listing.Label("Yockay_Atropos.CycleDaysExplain1".Translate());
      listing.Label("Yockay_Atropos.CycleDaysExplain2".Translate());
      listing.TextFieldNumericLabeled(
        $"{"Yockay_Atropos.CycleDays".Translate()}  ",
        ref this.cycleDays,
        ref buffer,
        0.1f,
        10000f
      );
      listing.Gap();

      buffer = null;
      listing.Label("Yockay_Atropos.AnnounceBeforeDaysExplain1".Translate());
      listing.Label("Yockay_Atropos.AnnounceBeforeDaysExplain2".Translate());
      listing.TextFieldNumericLabeled(
        $"{"Yockay_Atropos.AnnounceBeforeDays".Translate()}  ",
        ref this.announceBeforeDays,
        ref buffer,
        0f,
        this.cycleDays / 2
      );
      listing.Gap();

      listing.CheckboxLabeled("Yockay_Atropos.debugMode".Translate(), ref this.debugMode);

      listing.End();
    }
  }
}