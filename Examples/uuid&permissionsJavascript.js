function PlayerJoin(name) //onJoin(name) is invoked when a player with name name joins the server
{
	if(!Permission.HasPermission(name, "examples.third.beOnline")) //check if player has permission to be on server
		Server.RunCommand("kick "+name+" You dont have the Permission to join this server!"); //kick him if not
}
function OnCommand(name, arg) //onCommand(name, arg) is invoked when a player writes something in chat beginnign with '!'
{
	//check if command is getuuid and player has permission to get his uuid
	if(arg.split(' ')[0].toLowerCase() == "getuuid" && Permission.HasPermission(name, "examples.third.getUuid"))
		Player.SendMessageTo(name, "Your UUID is "+/*get uuid by name*/Uuid.GetUuidOf(name), "yellow"); //tell player his uuid
}
