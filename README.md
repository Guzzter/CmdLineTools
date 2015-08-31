# CmdLineTools
Some small console apps (mainly for automating files and directory operations)
Note: some projects uses some package from NuGet (like CommandLineParser and Newtonsoft.Json)

#### CopyFilesMatchingDirSuffix
Search for files in [sourcedir] and tries to determine in which subdir of the [targetdir] it should be copied.
Does a testrun first and then asks for permission

Example subdir in [targetdir]: 'e:\targetdir\series - South Park (sp.,sp-,south)'
Extracted rules: Move every file that
 - contains 'South Park' or 'SouthPark' or 'South-Park' or 'South_Park' or 'South.Park' 
 - starts with 'sp.' or 'sp-' or 'south'
Note that matches are case-insensitive and that 'series - ' is not mandatory and is not used.

Syntax: CopyFilesMatchingDirSuffix [sourcedir] [targetdir]

#### FileSyncher
Util for synching files easily based on a configuration file.
Copies or move files from one directory to another. Also two-way synch copying is possible.
It is possible to keep existing files or to overwrite them with the new file.

#### RemoveBlackListedFiles
Removes all files matching a certain pattern. Patterns are managed in a XML file (see BlackList.xml) 
Options:
-c, --config            (Default: RemoveBlackListedFiles.xml) Only show actions verbosely, do not perform actions.
-r, --recursive(Default: False) Also search in subdirectories?
-d, --directory The directory that needs to be processed.
-p, --performactions    (Default: False) Do really perform actions, use only when you're 100% sure!
--help Display this help screen.


#### RemoveEmptyDirectories
Utility to remove empty directories. When no directory specified the current directory is used.
Run RemoveEmptyDirectories -? for help. 
Options:
    -r, --recursive         (Default: False) Also search in subdirectories?
    -d, --directory The directory that needs to be processed.
    -p, --performactions    (Default: False) Do really perform actions, use only when you're 100% sure!
    --help Display this help screen.
            
Example syntax: RemoveEmptyDirectories -recursive
When -p is not specified, a testrun is done


#### RenameRecursively
Utility to rename files recursively
See RenameRecursively --help for options

#### SyncDirNames
Search for directories in [sourcedir] and creates them in [targetdir] when not existing.
Does a testrun first and then asks for permission

Syntax: SyncDirNames [sourcedir] [targetdir]

