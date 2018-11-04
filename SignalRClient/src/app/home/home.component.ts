import { EnvironmentUrlService } from './../shared/environment-url.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  private _hubConnection: HubConnection | undefined;
  private readonly fullPath: string;
  public async: any;
  message = '';
  messages: string[] = [];
  constructor(private environmentUrlService: EnvironmentUrlService) {
    this.fullPath = `${this.environmentUrlService.urlAddress}/hub/loopy`;
  }

  public sendMessage(): void {
    const data = `Sent: ${this.message}`;

    if (this._hubConnection) {
      this._hubConnection.invoke('Send', data);
    }
    this.messages.push(data);
  }

  ngOnInit() {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.fullPath)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().catch(err => console.error(err.toString()));

    this._hubConnection.on('Send', (data: any) => {
      const received = `Received: ${data}`;
      this.messages.push(received);
    });
  }
  ngOnDestroy() {
    this._hubConnection.stop();
    // Called once, before the instance is destroyed.
    // Add 'implements OnDestroy' to the class.
  }
}

