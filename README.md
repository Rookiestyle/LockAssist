# LockAssist
[![Version](https://img.shields.io/github/release/rookiestyle/lockassist)](https://github.com/rookiestyle/lockassist/releases/latest)
[![Releasedate](https://img.shields.io/github/release-date/rookiestyle/lockassist)](https://github.com/rookiestyle/lockassist/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/rookiestyle/lockassist/total?color=%2300cc00)](https://github.com/rookiestyle/lockassist/releases/latest/download/lockassist.plgx)\
[![License: GPL v3](https://img.shields.io/github/license/rookiestyle/lockassist)](https://www.gnu.org/licenses/gpl-3.0)

LockAssist extends KeePass' lock/unlock mechanism in multiple ways:  
- Quick Unlock
- Soft Lock (privacy mode)
- (Un)lock workspace can handle ALL databases or only the selected one

# Table of Contents
- [Configuration](#configuration)
- [Usage](#usage)
- [Translations](#translations)
- [Download and Requirements](#download-and-requirements)

# Configuration
LockAssist integrates into KeePass' options form.\
<img src="images/LockAssist%20-%20options.png" alt="Options" height="50%" width="50%"/>  

# Usage
You'll find more details in the [Wiki](https://github.com/rookiestyle/lockassist/wiki)

## Quick Unlock
Quick Unlock lets you unlock a previously opened database without the need to enter the complete password.  
You can unlock your database with a only a few characters instead - your quick unlock key.  
Have a look at the [Wiki](https://github.com/rookiestyle/lockassist/wiki/quick-unlock) to learn why this does not impose security risks.

<img src="images/LockAssist%20-%20quick%20unlock.png" alt="Options" height="50%" width="50%"/>  

## SoftLock
SoftLock hides sensitive information while still allowing Auto-Type as well as other integration.  
You can configure SoftLock to kick in after a certain inactivity or when minimizing KeePass.  

When active, the following is hidden:
- group list  
- entry list  
- entry view  
- all forms containing sensitive data (entry form, ...)

<img src="images/LockAssist%20-%20softlock.png" alt="Options" height="50%" width="50%"/>  

## Lock workspace
By default, 'Lock workspace' will lock all loaded databases whereas 'Unlock workspace' will only unlock the currently selected database.  

LockAssist changes the behaviour of 'Lock Workspace' for both the menu entry as well as the toolbar button.  
If this option is active ALL loaded databases are locked / unlocked by using these commands.  
It depends on the active document's state whether a global lock or global unlock is performed.

Hold the [Shift] key pressed to only lock / unlock the active document.

# Translations
LockAssist is provided with english language built-in and allows usage of translation files.
These translation files need to be placed in a folder called *Translations* inside in your plugin folder.
If a text is missing in the translation file, it is backfilled with the english text.
You're welcome to add additional translation files by creating a pull request.

Naming convention for translation files: `lockassist.<language identifier>.language.xml`\
Example: `lockassist.de.language.xml`
  
The language identifier in the filename must match the language identifier inside the KeePass language that you can select using *View -> Change language...*\
This identifier is shown there as well, if you have [EarlyUpdateCheck](https://github.com/rookiestyle/earlyupdatecheck) installed

# Download and Requirements
## Download
Please follow these links to download the plugin file itself.
- [Download newest release](https://github.com/rookiestyle/lockassist/releases/latest/download/LockAssist.plgx)
- [Download history](https://github.com/rookiestyle/lockassist/releases)

If you're interested in any of the available translations in addition, please download them from the [Translations](Translations) folder.
## Requirements
* KeePass: 2.41

