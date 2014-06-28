function PlayerJoin(name) //PlayerJoin(name) is invoked when a player with name name joins the server
{
	Console.WriteLine("Name '"+name+"'");
	Player.SendMessageTo(name, "Welcome to this server!", "gold"); //Send a welcome message to the new Player
}

function PlayerLeave(name) //PlayerLeave(name) is invoked when a player with name name leaves the server
{
	Console.WriteLine("Name '"+name+"'");
	Console.Beep(); //play a beep sound
}

function ChatReceived(name, message) //ChatReceived(name, message) is invoked when a Player says something
{
	Console.WriteLine("Name '"+name+"' --- '"+message+"'");
	if(message.toUpperCase() == message && message.length > 3) //check if message is caps only and longer than 3 characters long
		Server.RunCommand("kick "+name+" Please do not use caps lock!") //kick player cause he used caps
}

function OnCommand(name, arg) //OnCommand(name, args) is invoked when a player says something beginning with an '!'
{
	Console.WriteLine("Name '"+name+"' -c- '"+arg+"'");
	var args = arg.split(' '); //convert arg string to string array
	if(args[0] == "tpme") //check if command is 'tpme'
		Server.RunCommand("tp "+args[1]+" "+name); //teleport sender to named player
	else if(args[0] == "tpto") //check if command is 'tpto'
		Server.RunCommand("tp "+name+" "+args[1]); //teleport named player to sender
	else if(args[0] == "light") //check if command is 'light'
		Server.RunCommand("tp "+name+" ~ ~ ~") //you can use this command to call onPosition at the current position of the player
}

function PlayerPosition(name, position) //PlayerPosition is invoked when a player is teleported to specific coordinates
{ //position is a ready to use string for command like tp or setblock (e. g. '10.3356 125.1234 46.6368')
	Server.RunCommand("setblock "+position+" minecraft:torch"); //set block where the player stands to a torch so the player has light
}

function ServerStart() //ServerStart() is invoked when the server starts
{
	Console.WriteLine("Example Plugin active!\nCommands: !tpto <player>, !tpme <player> and !light\nThis Plugin has an anti-caps lock function!"); //display message to server log
}

function ServerStop() //ServerStop() is invoked when the server stops
{
	Console.Beep(); //note that if you play this too often java throws an exception cause it waited to long for the server Gui
}