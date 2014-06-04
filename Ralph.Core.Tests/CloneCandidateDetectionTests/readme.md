CloneCandidateDetectionTests is the full 'integration' test. 

These tests use a fully parsable c# file as test definitions. The pattern, matching
text, and repair text are each defined in the file.

There will be a section in the file delimited with /* BEGIN */ and /* END */ 
comments. This defines the clone code (matching text). The matching of the pattern
must return the 'coordinates' of this text block.

The pattern is not explicitely defined. The test turns all methods into patterns
and then applies them each in turn. ** TODO: This should be changed so that
there is a single pattern, and it is called out explicitely. **

The repair text takes form of a method call. It is defined inside the first
delimiting comment block, like so: /* BEGIN pat('a',7) */