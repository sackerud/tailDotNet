tailDotNet
==========

An implementation of the UNIX tail command in .Net.

If you just want to try it out I recommend installing tailDotNet using Chocolatey: https://chocolatey.org/packages/taildotnet. Simply open a command prompt with administrative privileges and type
    `choco install taildotnet`

Start tailing a file by typing `tail -f someFileName.log` in a command prompt.

Only interested of changes that contains "error"? Use the includefilter parameter:

`tail -f someFileName.log --includefilter error` or the shorthand form `tail -f someFileName.log -i error`

You can also exclude stuff from the tail output by using the excludefilter. Like so:

`tail -f someFileName.log --excludefilter aCommonErrorMessageThatOccursAllTheTimeButYouDontBotherToFix`
