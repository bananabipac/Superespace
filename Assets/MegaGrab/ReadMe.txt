Mega Grab is a script that allows you to grab upscaled and anti aliased screenshots. If you ever need to do real prints of your game for posters or banners, or need to grab an even more anti aliased image then this script will do that. You say how many times you want the grab to be bigger than the actual rendered size and how many extra levels of anti aliasing you require and when you grab Mega Grab will automatically render, upscale and antialias the camera to a file.

Please note if you are upscaling a lot and with many AA samples it could take a few minutes for a grab to happen.

The params in the script are:
SrcCamera		- camera to use for screenshot
GrabKey			- Key to grab the screenshot
ResUpscale		- How much to increase the screen shot res by
Blur			- Pixel oversampling. Use to slightly blur the AA samples if you still notice artifacts.
AASamples		- Anti aliasing samples.
FilterMode		- Filter mode. Can be used to turn of filtering if upscaling.
UseJitter		- Use random jitter for AA sampling. Mega Grab will use a grid array for sampling but this can lead
				  to artifacts on noisy images to try setting this for a random array
SaveName		- Base name for grabs
Format			- format string for date time info
Path			- Path to directory to save files in, must have a trailing '/' if left as "" then will save in the
				  game dir.
UseDOF			- Turn on DOF grab
totalSegments	- How many samples to take around the dof camera circle
sampleRadius	- radius of the DOF camera circle
		
UseDOF			- Use Dof grab
focalDistance	- DOF focal distance
totalSegments	- How many DOF samples
sampleRadius	- Amount of DOF effect
CalcFromSize	- Let grab calc res from dpi and Width(in inches)
Dpi				- Number of Dots per inch required
Width			- Final physical size of grab using Dpi
NumberOfGrabs	- Read only of how many grabs will happen
EstimatedTime	- Guide to show how long a grab will take in Seconds
GrabWidthWillBe	- Width of final image
GrabHeightWillBe- Height of final IMage
			  
Updates:
Changed system to grab to ram and not vram allowing for any size grabs just limited by memory.
Output is TGA for the moment until I get my Png encoder working properly.
Added early DOF support, needs testing.
I have added auto calc of the scale value if you provide Dpi and with values, also added values to show the output image size and how long the grab will take in seconds;

Plans:
Add uploader
			  
Any problems or suggestions please email me at chris@west-racing.com

Chris West
