import process from 'node:process'
import * as SignalR from '@microsoft/signalr'

/**
 * SignalRService
 * A facade for the server/client communication over the SignalR protocol.
 */
class SignalRService {
  constructor() {
    this.connected = false;
  }

  configureConnection() {
    const host = process.env.NODE_ENV == 'production' ? "/videoHub" : "https://localhost:7081/videoHub";
    this.connection = new SignalR.HubConnectionBuilder()
      .withUrl(`${host}`)
      .withAutomaticReconnect()
      .configureLogging(SignalR.LogLevel.Information)
      .build();
  } 

  async connect() {
    try {
      if (this.connection.state === SignalR.HubConnectionState.Disconnected) {
        await this.connection.start();
        this.connected = true;  
      }
    } catch (error) {
      console.error("Failed to connect to SignalR hub:", error);
      this.connected = false;
      throw error;
    }
  } 

  subscribeEvent(type, callback) {
    this.connection.on(type, callback);
  }

  unsubscribeEvent(type, callback) {
    if (typeof callback === 'undefined')
      this.connection.off(type);
    else
      this.connection.off(type, callback);
  } 

  async sendVideoCallToAll(videoData) {
    if (!this.connected) { throw new Error("Invalid state. Not connected."); }
    console.log(videoData.id);
    await this.connection.invoke("SendVideoCallToAll", videoData.id);
  }
}

const signalRSerivce = new SignalRService();
export default signalRSerivce;