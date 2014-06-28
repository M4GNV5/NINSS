#NINSS

NINSS stands for "NINSS is no serversoftware". NINSS is a Minecraft Server Wrapper that allows you to add plugins to any minecraft version.
It was designed to easily Write and import Plugins Furthermore it is ment be an opportunity for programming
beginners to improve their skills.

#Plugin Example code
```javascript
function PlayerJoin(name) //onJoin(name) is invoked when a player with name name joins the server
{
	Player.SendMessageTo(name, "Welcome to this server!", "gold"); //Send a welcome message to the new Player
}
function OnCommand(name, arg) //onCommand(name, args) is invoked when a player says something beginning with an '!'
{
  if(arg.split(' ')[0] == "leave") //if command is "leave"
    Server.WunCommand("kick "+name+" Own decision"); //kick player
}
function ServerStart() //onStart() is invoked when the server starts
{
  Console.WriteLine("Loaded <Pluginname>");
}
```

#License
NINSS is published under the 4 clause BSD license what means you can use source and binary for everything but your project needs to include the following clause: "This product includes software developed by Jakob Löw (M4GNV5)."
