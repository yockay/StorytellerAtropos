using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;


namespace Yockay.StorytellerAtropos
{
  public class StorytellerComp_OnOffCycle : RimWorld.StorytellerComp_OnOffCycle
  {
    // KTick = Kilo Tick = 1000 Tick = 0.4 game hour
    public static int PassedKTick => Find.TickManager.TicksSinceSettle / 1000;

    public static int GetRaidKTick(int passedCycleCount, int cycleDaysKTick)
    {
      return (passedCycleCount * cycleDaysKTick) + 1;
    }

    protected new StorytellerCompProperties_OnOffCycle Props => (StorytellerCompProperties_OnOffCycle)this.props;

    public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
    {
      bool incidentFlag = CheckIncidentsThisInterval(
        Core.settings.minDaysPassed,
        Core.settings.cycleDays,
        Core.settings.announceBeforeDays
      );

      if (incidentFlag)
      {
        FiringIncident firingIncident = this.GenerateIncident(target);
        if (firingIncident != null)
        {
          yield return firingIncident;
        }
      }
      yield break;
    }

    public static bool CheckIncidentsThisInterval(float minDaysPassed, float cycleDays, float announceBeforeDays)
    {
      int minDaysPassedKTick = DaysToIntervals(minDaysPassed);
      int passedSinceMinDaysKTick = PassedKTick - minDaysPassedKTick;
      if (passedSinceMinDaysKTick < 0)
      {
        if (Core.settings.debugMode)
        {
          string logMessage = string.Format(
            "Yockay_Atropos minDaysPassed:{0} cycleDays:{1} announceBeforeDays:{2} minDaysPassedKTick:{3} passedSinceMinDaysKTick:{4}",
            minDaysPassed,
            cycleDays,
            announceBeforeDays,
            minDaysPassedKTick,
            passedSinceMinDaysKTick
          );
          Log.Message(logMessage);
        }
        return false;
      }

      int cycleDaysKTick = DaysToIntervals(cycleDays);
      int passedCycleCount = passedSinceMinDaysKTick / cycleDaysKTick;
      int raidKTick = GetRaidKTick(passedCycleCount, cycleDaysKTick);

      // raid announce
      int announceKTick = 0;
      int nextRaidKTick = GetRaidKTick(passedCycleCount + 1, cycleDaysKTick);
      if (announceBeforeDays > 0f)
      {
        announceKTick = nextRaidKTick - DaysToIntervals(announceBeforeDays);
        if (announceKTick == nextRaidKTick)
        {
          announceKTick--;
        }
        if (announceKTick == passedSinceMinDaysKTick)
        {
          float period = announceBeforeDays;
          TaggedString taggedString = "Yockay_Atropos.RaidAnnounceDays".Translate(string.Format("{0:F0}", period));
          if (period < 1f)
          {
            period *= 24f;
            taggedString = "Yockay_Atropos.RaidAnnounceHour".Translate(string.Format("{0:F0}", period));
          }
          Messages.Message(taggedString, MessageTypeDefOf.NegativeEvent, false);
        }
      }

      if (Core.settings.debugMode)
      {
        string logMessage = string.Format(
          "Yockay_Atropos passedSinceMinDaysKTick:{0} raidKTick:{1} nextRaidKTick:{2} announceKTick:{3}",
          passedSinceMinDaysKTick,
          raidKTick,
          nextRaidKTick,
          announceKTick
        );
        Log.Message(logMessage);
      }

      return raidKTick == passedSinceMinDaysKTick;
    }

    public static int DaysToIntervals(float days)
    {
      return Mathf.RoundToInt(days * 60f);
    }

    private FiringIncident GenerateIncident(IIncidentTarget target)
    {
      if (this.Props.IncidentCategory == null)
      {
        return null;
      }
      IncidentParms parms = this.GenerateParms(this.Props.IncidentCategory, target);
      return !base.UsableIncidentsInCategory(this.Props.IncidentCategory, parms).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out IncidentDef def)
        ? null
        : new FiringIncident(def, this, parms);
    }

    public override string ToString()
    {
      return this.Props.IncidentCategory == null
        ? ""
        : base.ToString() + " (" + this.Props.IncidentCategory.defName + ")";
    }
  }
}
