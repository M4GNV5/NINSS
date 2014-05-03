function onJoin(name) //onJoin(name) is invoked when a player with name name joins the server
{
	if(!Permission.hasPermission(name, "examples.third.beOnline")) //check if player has permission to be on server
		Server.runCommand("kick "+name+" You dont have the Permission to join this server!"); //kick him if not
}
function onCommand(name, arg) //onCommand(name, arg) is invoked when a player writes something in chat beginnign with '!'
{
	//check if command is getuuid and player has permission to get his uuid
	if(arg.split(' ')[0].toLowerCase() == "getuuid" && Permission.hasPermission(name, "examples.third.getUuid"))
		Player.sendMessageTo(name, "Your UUID is "+/*get uuid by name*/Uuid.getUuidOf(name), "yellow"); //tell player his uuid
}
