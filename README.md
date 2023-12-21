# FFXIIIWMPtool
This tool allows you to unpack and repack the WMP movie archive files from FINAL FANTASY XIII.

The program should be launched from command prompt with any one of these following argument switches along with the movie_items.win32.wdb file and the WMP file: 
<br>``-u`` Unpacks a WMP file
<br>``-r`` Repacks the unpacked WMP folder to a valid WMP file

Commandline usage examples:
<br>``FFXIIIWMPtool.exe -u "movie_items.win32.wdb file" "z002.win32.wmp"``
<br>``FFXIIIWMPtool.exe -r "movie_items.win32.wdb file" "_z002.win32.wmp"``

### Important
- When unpacking and repacking english voice over movie files, make sure to check if both the wdb file and the WMP file has a ``_us`` appended to the filename just before the extension. for the english voiceover files, the filenames would be ``movie_items_us.win32.wdb`` and ``z002_us.win32.wmp``. the above usage example uses the Japanese voiceover files of these two files.
- The PC and the Xbox 360 versions will unpack the movie files with a ``.bik`` extension while the PS3 version movies will unpack with a ``.pam`` extension. the ``.bik`` files can be played via RADtools software while the ``.pam`` files can be played in
  the PS3StreamViewer app included with Sony's PS3PAMF Tools suite.
