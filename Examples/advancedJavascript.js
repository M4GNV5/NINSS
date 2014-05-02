/*Default Config:

<?xml version="1.0" encoding="UTF-8"?>
<root>
  <Welcome_Message>Use !welcome [message] and !welcome_color [color] to set the welcome message and color</Welcome_Message>
  <Welcome_Message_color>red</Welcome_Message_color>
  <Max_UpperCase_percent>40</Max_UpperCase_percent>
</root>

*/

function onStart() //onStart() is invoked when the server starts
{
	refreshValues(); //see below
}

function refreshValues()
{
	//configs are saved as .xml files in the ./plugins/configs/ folder
	Config.loadConfig("advancedJavascript"); //load config file named 'advancedJavascript'

	//Welcome Message:
	//Var can be used to save variables temporally to use them in other functions (usage Var.set(name, value); and Var.get(name); )
	Var.set("advancedJs_welcome", /*get config value named 'Welcome_message'*/Config.getValue("Welcome_Message")); //set value of 'advancedJS_welcome' to the value we get from the config
	Var.set("advancedJs_welcome_color", /*get config value named 'Welcome_message_color'*/Config.getValue("Welcome_Message_color")); //set value of 'advancedJS_welcome_color' to the value we get from the config

	//Anti Caps:
	Var.set("advancedJs_caps_percent", /*get config value named 'Welcome_message_color'*/Config.getValue("Max_UpperCase_percent")); //set value of 'advancedJs_caps_percent' to the value we get from the config
}

function onCommand(name, arg) //onCommand(name, args) is invoked when a player says something beginning with an '!'
{
	var args = arg.split(' '); //convert arg string to string array
	if(args[0] == "welcome" && args.length > 1) //if command is 'welcome'
	{
		//get welcome message
		var welcome = args[1];
		if(args.length > 2)
			for(var i = 2; i < args.length; i++)
				welcome += " "+args[i];
		Config.setValue("Welcome_Message", welcome); //set new welcome message in config
		Config.saveConfig("advancedJavascript"); //save the config
		refreshValues(); //refresh the variables
		Player.sendMessageTo(name, "Welcome Message set!", "dark_blue"); //send sucess message to sender
	}
	else if(args[0] == "welcome_color" && args.length == 2) //if command is 'welcome_color'
	{
		Config.setValue("Welcome_Message_color", args[1]); //set new welcome message color in config
		Config.saveConfig("advancedJavascript"); //save the config
		refreshValues(); //refresh the variables
		Player.sendMessageTo(name, "Welcome Message Color set to this color!", args[1]); //send sucess message to sender
	}
	else if(arg[0] == "welcome_refresh") //if command is 'welcome_refresh'
		refreshValues(); //refresh the variables
}

function onJoin(name) //onJoin(name) is invoked when a player with name name joins the server
{
	Player.sendMessageTo(name, Var.get("advancedJs_welcome"), Var.get("advancedJs_welcome_color")); //Send the welcome message to the new Player
}

function onChat(name, message) //onChat(name, message) is invoked when a Player says something
{
	var upper = 0; //temporary variable with upper case letter count
	for(var i = 0; i < message.length; i++) //loop through all letters in message
		if(message.substring(i, i+1).toUpperCase() == message.substring(i, i+1)) //if letter is uppercase letter
			upper++; //add 1 to uppercase
	if(upper/message.length*100 > Var.get("advancedJs_caps_percent")) //if uppercase percentage is more than allowed value
		Server.runCommand("kick "+name+" A maximum of "+Var.get("advancedJs_caps_percent")+"% Upper Case Letters is allowed you had "+upper/message.length*100+"%"); //kick player
}