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
	msbuild .\AgentRalph.proj
}
function test_project {
	"The 'test' command has not been created.  Edit your project-stuff.psm1 file and add your project specific command(s) to the test_project function."
}
function clean_project {
	msbuild .\AgentRalph.proj /t:Clean
	rm-orig
}
function rebuild_project {
	msbuild .\AgentRalph.proj /t:Rebuild
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