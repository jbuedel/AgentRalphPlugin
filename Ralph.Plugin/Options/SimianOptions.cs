using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Application;
using JetBrains.Application.Components;
using JetBrains.Application.Configuration;
using JetBrains.ComponentModel;
using JetBrains.Util;

namespace AgentRalph.Options
{
    [ShellComponentInterface(ProgramConfigurations.VS_ADDIN)]
    [ShellComponentImplementation]
    public class SimianOptions : IXmlExternalizableShellComponent
    {
        private static readonly string[] DefaultExcludesList = new[]
                                                                  {
                                                                      "*.*proj",
                                                                      "*.sln",
                                                                      "*.user",
                                                                      "*.resx",
                                                                      "*.dll",
                                                                      "*.exe",
                                                                      "*.xsd",
                                                                      "*.xml",
                                                                      "*.Designer.*"
                                                                  };

        private static readonly string[] DefaultIncludesList = new[] {"*" /*"*"*/};

        public SimianBooleanOption[] CommandLineOptions
        {
            get
            {
                // REFACTOR: Can this lazy initialization go into the Init() method?
                if(commandLineOptions == null)
                {
                    // TODO: Consider including the long descrips at http://www.redhillconsulting.com.au/products/simian/index.html
                    List<SimianBooleanOption> clo = new List<SimianBooleanOption>();
                    clo.Add(new SimianBooleanOption("balanceCurlyBraces", "Account for curly braces when breaking lines"));
                    clo.Add(new SimianBooleanOption("balanceParentheses", "Account for parentheses when breaking lines"));
                    clo.Add(new SimianBooleanOption("balanceSquareBrackets", "Account for square brackets when breaking lines"));
                    clo.Add(new SimianBooleanOption("ignoreCharacterCase", "Match character literals irrespective of case"));
                    clo.Add(new SimianBooleanOption("ignoreCharacters", "Completely ignore character literals"));
                    clo.Add(new SimianBooleanOption("ignoreCurlyBraces", "Completely ignore curly braces"));
                    clo.Add(new SimianBooleanOption((string) "ignoreIdentifierCase", (string) "Match identifiers irresepctive of case"));
                    clo.Add(new SimianBooleanOption("ignoreIdentifiers", "Completely ignore identifiers"));
                    clo.Add(new SimianBooleanOption("ignoreLiterals","Completely ignore all literals (strings, numbers and characters)"));
                    clo.Add(new SimianBooleanOption((string) "ignoreModifiers", (string) "Ignore modifiers (public, private, static, etc.)"));
                    clo.Add(new SimianBooleanOption("ignoreNumbers", "Completely ignore numbers"));
                    clo.Add(new SimianBooleanOption("ignoreRegions", "Ignore all lines between #region/#endregion"));
                    clo.Add(new SimianBooleanOption((string) "ignoreStringCase", (string) "Match string literals irrespective of case"));
                    clo.Add(new SimianBooleanOption("ignoreStrings", "Completely ignore the contents of strings"));
                    clo.Add(new SimianBooleanOption("ignoreSubtypeNames","Match on similar type names (eg. Reader and FilterReader)"));
                    clo.Add(new SimianBooleanOption("ignoreVariableNames","Completely ignore variable names (fields, parameters and locals)"));
                    commandLineOptions = clo.ToArray();
                }
                return commandLineOptions;
            }
        }

        private SimianBooleanOption[] commandLineOptions;

        [XmlExternalizableAttribute(null)] public string PathToSimian;

        [XmlExternalizableAttribute(null)] public string[] SpecsToExclude = DefaultExcludesList;

        [XmlExternalizable(null)] public string[] SpecsToInclude = DefaultIncludesList;

        #region IXmlExternalizableShellComponent implementation

        public string TagName
        {
            get
            {
                // tag name, should not conflict with any other plugins and internal ReSharper components
                return "AgentRalph.ExploreSimianResults.SimianOptions";
            }
        }

        public XmlExternalizationScope Scope
        {
            get { return XmlExternalizationScope.WorkspaceSettings; }
        }

        public void ReadFromXml(XmlElement element)
        {
            if(element == null)
                return;

            foreach (SimianBooleanOption option in CommandLineOptions)
            {
                option.Value = Convert.ToBoolean(element.GetAttribute(option.Name));
            }

            // read values via reflection
            XmlExternalizationUtil.ReadFromXml(element, this);
        }

        public void WriteToXml(XmlElement element)
        {
            foreach (SimianBooleanOption option in CommandLineOptions)
            {
                element.SetAttribute(option.Name, option.Value.ToString());
            }

            // write values via reflection
            XmlExternalizationUtil.WriteToXml(element, this);
        }

        #endregion

        #region IShellComponent implementation

        public void Init()
        {
        }

        public void Dispose()
        {
        }

        #endregion
        public static SimianOptions Instance
        {
            get { return Shell.Instance.GetComponent <SimianOptions>(); }
        }

        public void AddSpecToExclude(string filespec)
        {
            SpecsToExclude = new List<string>(SpecsToExclude) {filespec}.ToArray();
        }
    }

    /// Currently am managing the serialization of SimianBooleanOptions manually.
    /// See <see cref="SimianOptions.WriteToXml"/>.
    public class SimianBooleanOption
    {
        public SimianBooleanOption(string name, string friendlyText)
        {
            Name = name;
            FriendlyText = friendlyText;
        }

        public bool Value { get; set; }

        public string FriendlyText { get; private set; }

        public string Name { get; private set; }
    }
}