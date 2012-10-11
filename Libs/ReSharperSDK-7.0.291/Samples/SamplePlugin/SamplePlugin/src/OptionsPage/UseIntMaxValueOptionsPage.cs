/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using JetBrains.Application.Settings;
using JetBrains.UI.Controls;

namespace JetBrains.ReSharper.SamplePlugin.OptionsPage
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using Annotations;
  using DataFlow;
  using Settings;
  using UI.CrossFramework;
  using UI.Options.Helpers;
  using UI.Options.OptionPages;
  using UI.Options;

  [OptionsPage(Pid, "Use Int Max Value Settings", typeof(SamplePluginThemedIcons.UseIntMaxValueOptions), ParentId = EnvironmentPage.Pid, Sequence = 100)]
  public class UseIntMaxValueOptionsPage : AOptionsPage
  {
    private const string Pid = "UseIntMaxValue";

    public UseIntMaxValueOptionsPage([NotNull] Lifetime lifetime, OptionsSettingsSmartContext settings)
      : base(lifetime, Pid)
    {
      if (lifetime == null) throw new ArgumentNullException("lifetime");

      Control = InitView(lifetime, settings);
    }

    private EitherControl InitView(Lifetime lifetime, OptionsSettingsSmartContext settings)
    {
      var grid = new Grid {Background = SystemColors.ControlBrush};

      var col1 = new ColumnDefinition {Width = GridLength.Auto};
      grid.ColumnDefinitions.Add(col1);
      var row1 = new RowDefinition {Height = GridLength.Auto};
      grid.RowDefinitions.Add(row1);

      var enabledBox = new CheckBoxDisabledNoCheck2 { Content = "Enable changing 2147483647 to int.MaxValue" };
      settings.SetBinding<UseIntMaxValueSettings, bool>(lifetime, x => x.Enable, enabledBox, CheckBoxDisabledNoCheck2.IsCheckedLogicallyDependencyProperty);

      Grid.SetRow(enabledBox, 0);
      Grid.SetColumn(enabledBox, 0);

      grid.Children.Add(enabledBox);
      return grid;
    }
  }
}