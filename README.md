AgentRalphPlugin
================

A code clone detection and repair plug-in for ReSharper.

## What is Agent Ralph? ##

Agent Ralph is primarily a code clone detection and repair tool for C#, delivered as a [ReSharper](http://www.jetbrains.com/resharper/) plug-in.  It also has a few other odds and ends features not related to code clones specifically, but are useful ReSharper extensions in their own right.

Agent Ralph will scan your source code and attempt to identify methods that are functional duplicates of another method.  Once a clone is identified Agent Ralph will prompt the user to replace the body of a method with a call to it's clone.  

In some cases two given methods can be _functionally equivalent_ (same inputs produce the same outputs and side effects) but not _textually equivalent_.  For example, two methods might differ only in the naming of local variables, and are otherwise identical.  Agent Ralph can detect this situation, and others like it, and determine that the methods are functionally equivalent.  

It does this by operating directly against C# syntax trees instead of textual source code.  Because of this, all whitespace and comments are ignored.  

Here's an example of [Agent Ralph in action] (https://code.google.com/p/agentralphplugin/wiki/CloneRepairExample)...

![alt](http://wiki.agentralphplugin.googlecode.com/git/CloneRepairExample.2.png)


...and a [list of features](https://code.google.com/p/agentralphplugin/wiki/ListOfFeatures).

### Primary Features ###
  * Method level [clone detection and repair](https://code.google.com/p/agentralphplugin/wiki/CloneRepairExample).


