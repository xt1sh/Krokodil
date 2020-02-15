import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install(Vue) {
    const connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/messages')
      .build()

    let messages = []

    connection.on("ReceiveMessage", function (userId, userName, message) {
      messages.push({ userId, userName, message })
    });

    connection.on("ReceiveSystemMessage", function(message) {
      messages.push({ message })
    })

    connection.start()
    connection.onclose(() => { })

    var value = "; " + document.cookie;
    var parts = value.split("; " + 'id' + "=");
    if (parts.length == 2)
      var userId = parts
        .pop()
        .split(";")
        .shift();
    var parts = value.split("; " + 'username' + "=");
    if (parts.length == 2)
      var userName = parts
        .pop()
        .split(";")
        .shift();

    Vue.prototype.$roomId = ''
    Vue.prototype.$messageHub = connection
    Vue.prototype.$messages = messages
    Vue.prototype.$userName = userName
    Vue.prototype.$userId = userId
  }
}
