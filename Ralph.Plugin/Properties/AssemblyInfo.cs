using System.Reflection;
using System.Runtime.InteropServices;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;
using JetBrains.UI.Application.PluginSupport;


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly : AssemblyTitle("AgentRalph")]
[assembly : AssemblyDescription("")]
[assembly : AssemblyConfiguration("Visual Studio 2008")]
[assembly : AssemblyCompany("Joshman")]
[assembly: AssemblyProduct("AgentRalph")]
[assembly : AssemblyCopyright("Copyright ©  2010")]
[assembly : AssemblyTrademark("")]
[assembly : AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly : ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly : Guid("7c6457ce-5735-49b7-bbbd-932e604c6a13")]

[assembly : PluginTitle("Agent Ralph")]
[assembly : PluginVendor("Josh Buedel")]
[assembly : PluginDescription("Code clone detection.")]

// Must match the resource name of the Actions.xml file.
[assembly : ActionsXml("AgentRalph.Actions.xml"
#if Resharper_4_5
    , Precompile = false
#endif
    )]