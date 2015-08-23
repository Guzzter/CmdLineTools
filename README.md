# CmdLineTools
Some small console apps (mainly for automating files and directory operations)

# Tools
## CopyFilesMatchingDirSuffix
Search for files in [sourcedir] and tries to determine in which subdir of the [targetdir] it should be copied.
Does a testrun first and then asks for permission

Example subdir in [targetdir]: 'e:\targetdir\series - South Park (sp.,sp-,south)'
Extracted rules: Move every file that
 - contains 'South Park' or 'SouthPark' or 'South-Park' or 'South_Park' or 'South.Park' 
 - starts with 'sp.' or 'sp-' or 'south'
Note that matches are case-insensitive and that 'series - ' is not mandatory and is not used.

Syntax: CopyFilesMatchingDirSuffix [sourcedir] [targetdir]

## SyncDirNames
Search for directories in [sourcedir] and creates them in [targetdir] when not existing.
Does a testrun first and then asks for permission

Syntax: SyncDirNames [sourcedir] [targetdir]
