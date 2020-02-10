import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install (Vue) {
    const connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/messages')
      .build()

      connection.on('ReceiveMessage', function (user, message) {
        console.log(user, message)
      })

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
      setTimeout(() => connection.invoke("JoinRoom", 'qqq'), 2000)
      setTimeout(() => connection.invoke('SendRoomMessage', 'qqq', 'www'), 5000)
  }
}
