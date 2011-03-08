# Unity Game Play Metrics #

Tracking and logging the game play metrics in [Unity 3D][].

## What this project is ##

This project grew out of a project done for the art school: [Trust in Me][1], a game done for an exhibition.

Since the game features some unusual mechanics, I was interested in trying to figure out how the players would react to it. Thus the Tracker (now "Unity Game Play Metrics") was born.

The given code allows you to track one game object (tagged "Player") and puts down its position, the current action (that you can define yourself) and the time into an XML file – a new one for every time the game is started.

[1]: http://xeophin.net/trust-me
[Unity 3D]: http://www.unity3d.com

## What this project is not (currently, anyway) ##

The code that is included will not allow you to visualise the results out of the box. In my case, I transformed the XML data to CSV and gave that to a friend, who did the final analysis in R.

How you want to visualise all this data is entirely up to you.

## Am I still working on it? ##

Not really – not at the moment, anyway. But this is exactly why I put it up on GitHub: feel free to fork it, improve the code – and I will try to integrate your changes back.

## How to Use ##

### LevelData.cs ###

Attach to a GameObject that stays in the level. It might be replaced in a future version, but is currently used as a way to hand some information to the tracker, so you are able to find what level and which version of it got tracked.

### SaveLocation.cs ###

Attach directly to the player – or another persistent GameObject. The class will try to look for a GameObject tagged `Player`.

Also, the script will look for a GameObject tagged with `Calibration`. This is used to give you a reference point in space. When combined with the start position of the `Player` or the point of origin, you should be able to scale any image data you produce in external tools onto your level.

Drag the GameObject that has the LevelData.cs attached to the LevelData field.

Now the player should be tracked.

### XML/xml2txt.xsl ###

For convenience's sake I included the XSL stylesheet I used to transform the XML data to CSV. Might not be that useful to you.

### Resulting XML ###

Due to the fact that in this project I used XML as well as C# for the first time, the resulting XML files are split into two: one is the *wrapper*, containing the start time, the name and version of the level as well as the calibration data. The other is the *log* that contains the actual logged data. The *log* file is referenced in the *wrapper* through an entity reference, most XML parsers should therefore not have a problem picking it up.

## License ##

UnityGamePlayMetrics by [Kaspar Manz | xeophin.net/worlds][2] is licensed under a [Creative Commons Attribution 3.0 Unported License][4].

Also, I do not claim that it will work on the first try. This is very much work done on the go, and was never really been intended to be released. But maybe with the help of other people, this could even get useful, who knows?

[2]: http://xeophin.net/unitygameplaymetrics
[4]: http://creativecommons.org/licenses/by/3.0/