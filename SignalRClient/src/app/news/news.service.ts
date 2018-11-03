import { EnvironmentUrlService } from './../shared/environment-url.service';
import { Observable, of, throwError, Subject } from 'rxjs';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { Injectable } from '@angular/core';

import { NewsTopicModel } from './news-topic/news-topic-model';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

// Constants
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
const WAIT_UNTIL_ASPNETCORE_IS_READY_DELAY_IN_MS = 2000;

@Injectable()
export class NewsService {
  // fields
  private subscribedTopics: NewsTopicModel[] = [];
  subscribedTopicsChanged = new Subject<NewsTopicModel[]>();
  private _hubConnection: HubConnection | undefined;
  private readonly apiUrl: string;
  private readonly hubUrl: string;

  private headers: HttpHeaders;
  connectionEstablished = new Subject<Boolean>();
  historyReceived = new Subject<string[]>();
  newsFeedReceived = new Subject<string>();

  constructor(
    private http: HttpClient,
    private environmentUrlService: EnvironmentUrlService
  ) {
    this.apiUrl = `${this.environmentUrlService.urlAddress}/api/news`;
    this.hubUrl = `${this.environmentUrlService.urlAddress}/news`;
    console.log(this.apiUrl);
    this.init();
    this.headers = new HttpHeaders();
    this.headers = this.headers.set('Content-Type', 'application/json');
    this.headers = this.headers.set('Accept', 'application/json');
  }

  getSubscribedNewsTopics() {
    return this.subscribedTopics.slice();
  }

  getSubscribedNewsTopic(id: number) {
    const myR = this.subscribedTopics.find(r => {
      return r.id === id;
    });
    return myR;
  }
  subscribe(topic: NewsTopicModel) {
    this.subscribedTopics.push(topic);
    this.subscribedTopicsChanged.next(this.subscribedTopics.slice());
    this.joinGroup(topic.name);
  }

  unSubscribe(topic: NewsTopicModel) {
    const index = this.subscribedTopics.indexOf(topic, 0);
    if (index > -1) {
      this.subscribedTopics.splice(index, 1);
      this.subscribedTopicsChanged.next(this.subscribedTopics.slice());
      this.leaveGroup(topic.name);
    }
  }

  generateNews(topicId: number) {
    const url = `${this.apiUrl}/GenerateNews`;
    return this.http.post<string>(url, topicId, httpOptions).pipe(
      tap(() => console.log(`news generated for Topic w/ id=${topicId}`)),
      catchError(this.handleError<string>('generateNews'))
    );
  }

  init() {
    this.createConnection();
    this.startConnection();

    this._hubConnection.on('History', (data: string[]) => {
      this.historyReceived.next(data);
    });

    this._hubConnection.on('NewsFeed', (data: string) => {
      this.newsFeedReceived.next(data);
    });
  }

  private joinGroup(topicName: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('JoinGroup', topicName);
    }
  }

  private leaveGroup(topicName: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('LeaveGroup', topicName);
    }
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .build();
  }

  private startConnection() {
    setTimeout(() => {
      this._hubConnection.start().then(() => {
        console.log('Hub connection started');
        this.connectionEstablished.next(true);
      });
    }, WAIT_UNTIL_ASPNETCORE_IS_READY_DELAY_IN_MS);
  }
}
