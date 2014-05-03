function onJoin(name) //onJoin(name) is invoked when a player with name name joins the server
{
	if(!Permission.hasPermission(name, "examples.third.beOnline"))
		Server.runCommand("kick "+name+" You dont have the Permission to join this server!");
}
function onCommand(name, arg)
{
	if(arg.split(' ')[0].toLowerCase() == "getuuid" && Permission.hasPermission(name, "examples.third.getUuid"))
		Player.sendMessageTo(name, "Your UUID is "+Uuid.getUuidOf(name), "yellow");
}