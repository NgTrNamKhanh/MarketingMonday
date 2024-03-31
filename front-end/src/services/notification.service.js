import { HubConnectionBuilder } from "@microsoft/signalr";
import apis from "./apis.service";
import { useState } from "react";

const connection = new HubConnectionBuilder()
    .withUrl(apis.normal+"notificationHub")
    .build();
connection.start().then(() => console.log("SignalR Connected."));

connection.on("ReceiveNotification", (message) => {
    console.log("Received notification:", message);
  // Handle the notification as needed (e.g., display it to the user)
});

export default connection;
