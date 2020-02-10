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

      let startedPromise = null
      let tries = 0
      function start () {
        startedPromise = connection.start().catch(err => {
          console.error('Failed to connect with hub', err)
          if (tries < 5) {
            return new Promise((resolve, reject) =>
              setTimeout(() => {
                start().then(resolve).catch(reject)
                tries++
                console.log('promise')
              }, 5000))
          }
        })
        tries = 0
        return startedPromise
      }
      connection.onclose(() => start())

      start()

      Vue.prototype.$messageHub = connection
      Vue.prototype.$messages = messages
  }
}
