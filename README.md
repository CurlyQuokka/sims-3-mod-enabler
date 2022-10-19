# sims-3-mod-enabler

Sims 3 Mod Enabler is a simple program that simplifies the process of mods enablement in The Sims 3 game.

## How to use

1. Put all your `.package` files in 
C:\Users\<USER>\Documents\Electronic Arts\The Sims 3\PackagesLibrary

2. Delete all files and directories in
C:\Users\<USER>\Documents\Electronic Arts\The Sims 3\Mods\Packages

3. Run the program, enable packages, save configuration and start the game. 

## Details

This program is really simple - it just reads the `PackagesLibrary` directory, makes list (tree in fact) of available packages, and once those are enabled and configuration is 'saved', creates hardlinks to enabled packages in `Mods\Packages` directory.

Only `.package` files are supported currently.

## Known issues

- Duplicated files might create problems.