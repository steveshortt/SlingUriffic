## Project Description
When installed as the default browser on a Windows machine, SlingUriffic will redirect incoming URLs to the browser of choice based on Regex pattern matches.

## What does SlingUriffic do, and how does it work?
SlingUriffic is designed to run as the default web browser on a Windows machine, and will handle all OS referrers to launch the browser. SlingUriffic is not a browser itself - rather, it redirects requests to the browser of choice based on a Regex dictionary match, as configured in the Settings file. The purpose for SlingUriffic is to provide web browser selection based on URL.

### Use cases
* Ensure sites that require IE for compatibility are directed to IE, and everything else to your preferred browser. This is handy in a corporate environment where IE is required for intranet compatibility.
* Always open certain URLs in "private" mode.

### Install/Uninstall
* The setup for SlingUriffic generates a .reg file that contains the settings to register it as the default web browser. You'll need to double-click the reg file to actually install SlingUriffic.
* To uninstall SlingUriffic, simply run whatever browser you wish to have the default browser, and make sure to have that browser detect if it is the default. The browser should prompt you to indicate that it is not the default browser and offer the opportunity to re-register itself. After that, you may delete SlingUriffic from your drive and there will be nothing left behind.
* See [documentation](https://github.com/steveshortt/SlingUriffic/wiki/About-SlingUriffic) for detailed instructions.


### Example Settings File:
```xml
<?xml version="1.0" encoding="us-ascii"?>
<Settings>
  <Browsers>
    <Browser Name="FF" Path="C:\Program Files (x86)\Mozilla Firefox\firefox.exe" IsDefault="true" />
    <Browser Name="IE" Path="C:\Program Files (x86)\Internet Explorer\iexplore.exe" Arguments="-nohome" />
  </Browsers>
  <Patterns>
    <Pattern BrowserName="IE" Match="codeplex.com" />
    <Pattern BrowserName="IE" Match="microsoft.com" />
  </Patterns>
</Settings>
```
In this case, codeplex.com and microsoft.com traffic is directed to Internet Explorer, and all other traffic will route to FireFox.

### Why write SlingUriffic?
I wrote this because I prefer other browsers to IE, but need IE for corporate compatibility. I found some utils on the web that do the same thing, but they installed with an .msi, and that makes me nervous. The code is simple and was fast to write, and I've been using it for a couple years on a few machines with no issues. If you run SlingUriffic, please review it. I'd like to know if you find it useful.
