function onJoin(name) //onJoin(name) is invoked when a player with name name joins the server
{
	Player.sendMessageTo(name, "Welcome to this server!", "gold"); //Send a welcome message to the new Player
}

function onLeave(name) //onLeve(name) is invoked when a player with name name leaves the server
{
	Console.Beep(); //play a beep sound
}

function onChat(name, message) //onChat(name, message) is invoked when a Player says something
{
	if(message.toUpperCase() == message && message.length > 3) //check if message is caps only and longer than 3 characters long
		Server.runCommand("kick "+name+" Please do not use caps lock!") //kick player cause he used caps
}

function onCommand(name, arg) //onCommand(name, args) is invoked when a player says something beginning with an '!'
{
	var args = arg.split(' '); //convert arg string to string array
	if(args[0] == "tpme") //check if command is 'tpme'
		Server.runCommand("tp "+args[1]+" "+name); //teleport sender to named player
	else if(args[0] == "tpto") //check if command is 'tpto'
		Server.runCommand("tp "+name+" "+args[1]); //teleport named player to sender
	else if(args[0] == "light") //check if command is 'light'
		Server.runCommand("tp "+name+" ~ ~ ~") //you can use this command to call onPosition at the current position of the player
}

function onPosition(name, position) //onPosition is invoked when a player is teleported to specific coordinates
{ //position is a ready to use string for command like tp or setblock (e. g. '10.3356 125.1234 46.6368')
	Server.runCommand("setblock "+position+" minecraft:torch"); //set block where the player stands to a torch so the player has light
}

function onStart() //onStart() is invoked when the server starts
{
	Console.WriteLine("\nExample Plugin active!\nCommands: !tpto <player>, !tpme <player> and !light\nThis Plugin has an anti-caps lock function!\n"); //display message to server log
}

function onStop() //onStart() is invoked when teh server stops
{
	Console.Beep(); //note that if you play this too often java throws an exception cause it waited to long for the server Gui
}