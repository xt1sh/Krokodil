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
      connection.onclose(() => {
        connection.on("LeaveRoom", Vue.prototype.$roomId)
        Vue.prototype.$axios.post(`http://localhost:5000/disconnect/zalup`)
      })

      Vue.prototype.$roomId = ''
      Vue.prototype.$messageHub = connection
      Vue.prototype.$messages = messages
  }
}
