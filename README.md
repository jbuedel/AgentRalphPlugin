AgentRalphPlugin
================

A code clone detection and repair plug-in for ReSharper.

## What is Agent Ralph? ##

Agent Ralph is primarily a code clone detection and repair tool for C#, delivered as a [http://www.jetbrains.com/resharper/ ReSharper] plug-in.  It also has a few other odds and ends features not related to code clones specifically, but are useful ReSharper extensions in their own right.

Agent Ralph will scan your source code and attempt to identify methods that are functional duplicates of another method.  Once a clone is identified Agent Ralph will prompt the user to replace the body of a method with a call to it's clone.  

In some cases two given methods can be _functionally equivalent_ (same inputs produce the same outputs and side effects) but not _textually equivalent_.  For example, two methods might differ only in the naming of local variables, and are otherwise identical.  Agent Ralph can detect this situation, and others like it, and determine that the methods are functionally equivalent.  

It does this by operating directly against C# syntax trees instead of textual source code.  Because of this, all whitespace and comments are ignored.  

Here's an example of [CloneRepairExample Agent Ralph in action] ...

[http://code.google.com/p/agentralphplugin/wiki/CloneRepairExample http://wiki.agentralphplugin.googlecode.com/git/CloneRepairExample.2.png]

...and a [ListOfFeatures list of features].

### Primary Features ###
  * Method level [CloneRepairExample clone detection and repair].

### Secondary Features ###
  * Browse the results of running [http://www.redhillconsulting.com.au/products/simian/ Simian] using the [SimianExplorer Simian Explorer].
  * Convert string based enum comparison to a type safe enum comparison [http://landofjosh.com/2009/05/unnecessary-complexity-case-study-1-untyped-enum-comparisons/ quick fix].

