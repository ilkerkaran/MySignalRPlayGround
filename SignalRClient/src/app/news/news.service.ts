
import { Observable, of, throwError, Subject } from 'rxjs';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { Injectable } from '@angular/core';

import { NewsTopicModel } from './news-topic/news-topic-model';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
const apiUrl = 'http://localhost:5000/api/news';
@Injectable()
export class NewsService {
  constructor(private http: HttpClient) {}

  subscribedTopicsChanged = new Subject<NewsTopicModel[]>();

  private subscribedTopics: NewsTopicModel[] = [];

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
  }

  generateNews(topicId: number) {
    const url = `${apiUrl}/GenerateNews`;
    return this.http.post<string>(url, topicId, httpOptions).pipe(
      tap(() => console.log(`news generated for Topic w/ id=${topicId}`)),
      catchError(this.handleError<string>('generateNews'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
