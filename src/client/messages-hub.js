import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install (Vue) {
    const connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/messages')
      .build()

    let messages = []

      connection.on("SendMessage", function(message) {
        messages.push(message)
      });

      connection.start()
      connection.onclose(() => console.log('closed'))

      Vue.prototype.$messageHub = connection
      Vue.prototype.$messages = messages
  }
}
