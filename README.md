#NINSS

NINSS stands for "NINSS is no serversoftware". NINSS is a Minecraft Server Wrapper that allows you to add plugins to any minecraft version.
It was designed to easily Write and import Plugins Furthermore it is ment be an opportunity for programming
beginners to improve their skills.

Binaries can be found here: https://github.com/M4GV5/NINSS/tree/master/NINSS/bin/Debug

#Plugin Example code
```javascript
function onJoin(name) //onJoin(name) is invoked when a player with name name joins the server
{
	Player.sendMessageTo(name, "Welcome to this server!", "gold"); //Send a welcome message to the new Player
}
function onCommand(name, arg) //onCommand(name, args) is invoked when a player says something beginning with an '!'
{
  if(arg.split(' ')[0] == "leave") //if command is "leave"
    Server.runCommand("kick "+name+" Own decision"); //kick player
}
function onStart() //onStart() is invoked when the server starts
{
  Console.WriteLine("Loaded <Pluginname>");
}
```

#License
NINSS is published under the 4 clause BSD license what means you can use source and binary for everything but your project needs to include the following clause: "This product includes software developed by Jakob Löw (M4GNV5)."
