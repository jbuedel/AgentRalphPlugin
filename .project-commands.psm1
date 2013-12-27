<#	
.Synopsis
My implementation of an command line for customized for the .net developer.  Specifically tailored to my preferred tools.
(msbuild, mercurial, Visual Studio, and TeamCity)

.Description
My implementation of an improved command line for the .net developer.  Specifically for those using my preferred tools.

Implement each of the following functions as you see fit for your project.  For example, the 'develop_project' function 
would likely open a .sln file, choosing the proper version of Visual Studio if necessary.  That function might look like
this:

	function develop_project {
		devenv MySolution.sln
	}

Note that when these functions are invoked, the current working directory is always set to the root of the project (which
is where this file is as well).  Once the command exits the user will be returned to the directory they were in.

This file should be checked into your project's source control.

#>

function build_project { 
	msbuild .\AgentRalph.proj /v:minimal
}
function test_project {
   &"c:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe" Ralph.Test.Project\Ralph.Test.Project.sln /ReSharper.Plugin Bin\Debug\Ralph.Plugin.dll
}
function clean_project {
	msbuild .\AgentRalph.proj /t:Clean /v:minimal
	rm-orig
}
function rebuild_project {
	#msbuild .\AgentRalph.proj /t:Rebuild /v:minimal
	write-host "*****************************************************************************"
	write-host "***Rebuild actually builds for release including making the nuget package.***"
	write-host "*****************************************************************************"
	msbuild .\AgentRalph.proj /t:Release /p:Configuration=Release /v:minimal /p:BUILD_NUMBER=3
}
function develop_project {
	.\AgentRalph.sln
}
function pushenv_project {
	"The 'pushenv' command has not been created.  Edit your project-stuff.psm1 file and add your project specific command(s) to the pushenv_project function."
}
function popenv_project {
	"The 'popenv' command has not been created.  Edit your project-stuff.psm1 file and add your project specific command(s) to the popenv_project function."
}

export-modulemember -function build_project
export-modulemember -function test_project
export-modulemember -function clean_project
export-modulemember -function rebuild_project
export-modulemember -function develop_project
export-modulemember -function pushenv_project
export-modulemember -function popenv_project
