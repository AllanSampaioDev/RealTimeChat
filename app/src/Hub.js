import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

export default class Hub {
    constructor(userId, userName) {
        this.connection = new HubConnectionBuilder()
        .withUrl('http://localhost:2020/chat-hub?userId=' + userId + '&userName=' + userName)
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build()
    }
}