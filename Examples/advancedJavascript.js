/*Default Config:

<?xml version="1.0" encoding="UTF-8"?>
<root>
  <Welcome_Message>Use !welcome [message] and !welcome_color [color] to set the welcome message and color</Welcome_Message>
  <Welcome_Message_color>red</Welcome_Message_color>
  <Max_UpperCase_percent>40</Max_UpperCase_percent>
</root>

*/
var welcomeMessage;
var welcomeColor;
var allowedCapsPercent;

function ServerStart() //ServerStart() is invoked when the server starts
{
	RefreshValues(); //see below
}

function RefreshValues()
{
	//configs are saved as .xml files in the ./plugins/configs/ folder
	Config.LoadConfig("advancedJavascript"); //load config file named 'advancedJavascript'

	//Welcome Message:
	welcomeMessage = Config.GetValue("Welcome_Message") || "Welcome to this server"; //set value of 'welcomeMessage' to the value we get from the config
	welcomeColor = Config.GetValue("Welcome_Message_color") || "red"; //set value of 'welcomeColor' to the value we get from the config
	allowedCapsPercent = Config.GetValue("Max_UpperCase_percent") || "40"; //set value of 'allowedCapsPercent' to the value we get from the config
}

function ChatReceived(name, message) //ChatReceived(name, message) is invoked when a Player says something
{
	var args = message.split(' ');

	if(args[0] == ".welcome" && args.length > 1) //if command is 'welcome'
	{
		//get welcome message
		var welcome = args[1];
		if(args.length > 2)
			for(var i = 2; i < args.length; i++)
				welcome += " "+args[i];
		Config.SetValue("Welcome_Message", welcome); //set new welcome message in config
		Config.SaveConfig("advancedJavascript"); //save the config
		RefreshValues(); //refresh the variables
		Player.SendMessageTo(name, "Welcome Message set!", "dark_blue"); //send sucess message to sender
	}
	else if(args[0] == ".welcome_color" && args.length == 2) //if command is 'welcome_color'
	{
		Config.SetValue("Welcome_Message_color", args[1]); //set new welcome message color in config
		Config.SaveConfig("advancedJavascript"); //save the config
		RefreshValues(); //refresh the variables
		Player.SendMessageTo(name, "Welcome Message Color set to this color!", args[1]); //send sucess message to sender
	}
	else if(args[0] == ".welcome_refresh") //if command is 'welcome_refresh'
		RefreshValues(); //refresh the variables

	var upper = 0; //temporary variable with upper case letter count
	for(var i = 0; i < message.length; i++) //loop through all letters in message
		if(message.substring(i, i+1).toUpperCase() == message.substring(i, i+1)) //if letter is uppercase letter
			upper++; //add 1 to uppercase
	if(message.length > 5 && upper/message.length*100 > allowedCapsPercent) //if uppercase percentage is more than allowed value
		Server.RunCommand("kick "+name+" A maximum of "+allowedCapsPercent+"% Upper Case Letters is allowed you had "+upper/message.length*100+"%"); //kick player
}

function PlayerJoin(name) //PlayerJoin(name) is invoked when a player with name name joins the server
{
	Player.SendMessageTo(name, welcomeMessage, welcomeColor); //Send the welcome message to the new Player
}