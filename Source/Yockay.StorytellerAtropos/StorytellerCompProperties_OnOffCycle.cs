using RimWorld;
using System.Collections.Generic;

namespace Yockay.StorytellerAtropos
{
  public class StorytellerCompProperties_OnOffCycle : RimWorld.StorytellerCompProperties_OnOffCycle
  {
    public IncidentCategoryDef category;

    public new IncidentCategoryDef IncidentCategory => this.incident != null ? this.incident.category : this.category;

    public StorytellerCompProperties_OnOffCycle()
    {
      this.compClass = typeof(StorytellerComp_OnOffCycle);
    }

    // 不要そうだが、親の ConfigErrors をオーバーライドする必要があるので、残しておくこと
    // オーバーライドしないと onDays の設定がない、と怒られる
    public override IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
    {
      if (this.incident != null && this.category != null)
      {
        yield return "incident and category should not both be defined";
      }
      yield break;
    }
  }
}
