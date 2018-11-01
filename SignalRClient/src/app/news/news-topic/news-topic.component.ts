import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { NewsTopicModel } from './news-topic-model';

@Component({
  selector: 'app-news-topic',
  templateUrl: './news-topic.component.html',
  styleUrls: ['./news-topic.component.css']
})
export class NewsTopicComponent implements OnInit, OnDestroy {
  @Input()
  topic: NewsTopicModel;
  messages: string[] = [];
  history: string[] = [];
  constructor() {}

  ngOnInit() {}

  ngOnDestroy() {}
}
