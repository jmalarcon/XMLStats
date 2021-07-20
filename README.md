# XMLStats

Cycles through XML files in a folder and spits out stats about all of them, including the number of words in the text nodes.

I needed something like this to count the number of words in .xml files with language resources. Since I didn't find anything right of the bat, I programmed this simple command line app.

Usage:

```bash
xmlstats pathToFolder
```

It shows the progress with the files that is analyzing (it's only visible if there are thousands of files), and shows some useful numbers at the end:

![Sample output](xmlstats-output-linux.png)

It's superfast analyzing thousands of .xml files (i.e: just 6.4ms analyzing the 151 files in the previous screenshot with more than 10 000 nodes). 

**Doesn't count words in comments.**

Works in Windows, Linux and Mac (untested).

Doesn't show the progress if you're redirecting the output to a file (i.e.: `XMLStats.exe C:\XML > results.txt`)

**The release files doesn't include the .NET 5 runtime**, so you'll need the runtime installed for it to work. For instructions on how to install .NET in the different operating systems, please [read this](https://docs.microsoft.com/en-us/dotnet/core/install/).

