Maya Extensions 0.24 readme
=================
More and up to date information available on http://adammechtley.com

Installing Maya Extensions
-----------------

Requirements: Unity Pro, Autodesk Maya 8.5 or newer

Some setup required, and then everything will just work.

1. Unzip AM_Tools.zip in Python Modules folder and install its contents in Maya
according to the instructions in the zip file. Confirm proper installation by
importing the amTools module in Maya.

2. Modify Unity's FBXMayaExport.mel script according to the instructions in the
Detailed Description section in the online documentation:

http://bit.ly/amTools_overview  

The FBXMayaExport.mel script can be found in the Tools folder in your Unity
install directory. (On OSX, you must right-click Unity.app and select Show
Contents in order to browse to the Tools folder.)

3. You can now enable and disable individual options from the AM Tools menu in
Maya, and the corresponding data will be automatically imported into Unity when
you save a Maya binary or Maya ASCII file in your project! Alternatively, you
can invoke Prep for Manual FBX Export from the menu and then export the file to
FBX. Both methods will allow the data to be imported correctly.

Because you have full source code for this tool, feel free to modify it if you
like, but PLEASE inform me of any bugs that you find so I can keep the tool up-
to-date. Have fun!

Adam Mechtley
adam@adammechtley.com