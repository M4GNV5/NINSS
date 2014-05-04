function loadPlugins(callback)
{
  if(document.pluginList)
    return;
  document.pcallback = callback;
  $.get('?plugins', function(data)
    {
      document.pluginList = new Array(data.split(',').length);
      for(var i = 0; i < data.split(',').length; i++)
        document.pluginList[i] = data.split(',')[i];
      setTimeout(document.pcallback, 1);
    });
}
function loadConfigs(callback)
{
  if(document.configList)
    return;
  document.ccallback = callback;
  $.get('?configlist', function(data)
    {
      document.configList = new Array(data.split(',').length);
      for(var i = 0; i < data.split(',').length; i++)
      {
        document.configList[i] = data.split(',')[i];
        document.getElementById('configs').innerHTML += "<li><a href=\"Config.html?-?"+document.configList[i]+"\">"+document.configList[i]+"</a></li>";
      }
      setTimeout(document.ccallback, 1);
    });
}