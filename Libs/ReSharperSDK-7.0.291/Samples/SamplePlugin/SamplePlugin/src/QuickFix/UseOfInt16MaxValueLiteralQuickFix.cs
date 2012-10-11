/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License a t
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.SamplePlugin.DaemonStage;
using JetBrains.Util;

namespace JetBrains.ReSharper.SamplePlugin.QuickFix
{
  [QuickFix]
  public class UseOfInt16MaxValueLiteralQuickFix : IQuickFix
  {
    private readonly UseOfInt16MaxValueLiteralHighlighting _highlighting;

    public UseOfInt16MaxValueLiteralQuickFix([NotNull] UseOfInt16MaxValueLiteralHighlighting highlighting)
    {
      _highlighting = highlighting;
    }

    public bool IsAvailable(IUserDataHolder cache)
    {
      return _highlighting.IsValid();
    }

    public void CreateBulbItems(BulbMenu menu, Severity severity)
    {
      menu.ArrangeQuickFix(new UseOfInt16MaxValueLiteralBulbItem(_highlighting.Expression), severity);
    }
  }
}