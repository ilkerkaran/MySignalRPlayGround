import { Component, OnInit } from '@angular/core';
import { NewsTopicModel } from './news-topic/news-topic-model';
import { NewsService } from './news.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {
  constructor(public newsService: NewsService) {}

  subscribedTopics: NewsTopicModel[];
  subTopicChangedSub: Subscription;

  selectedTopicId = 0;
  subscriptionCount = 0;
  ngOnInit() {
    this.subscribedTopics = this.newsService.getSubscribedNewsTopics();
    this.subTopicChangedSub = this.newsService.subscribedTopicsChanged.subscribe(
      topics => (this.subscribedTopics = topics)
    );
  }

  onNewsTopicChange(el) {
    this.selectedTopicId = el.target.value;
  }

  onSubscribe() {
    if (
      this.selectedTopicId > 0 &&
      !this.newsService.getSubscribedNewsTopics().find(i => {
        return i.id == this.selectedTopicId;
      })
    ) {
      this.newsService.subscribe(
        new NewsTopicModel(
          this.selectedTopicId,
          this.getTopicName(this.selectedTopicId)
        )
      );
      this.subscriptionCount++;
    }
  }

  onGenerateNews() {}

  getTopicName(topicId: number) {
    if (topicId == 1) return 'Politics';
    else if (topicId == 2) return 'Crime';
    else if (topicId == 3) return 'Technology';
    else if (topicId == 4) return 'World News';
    else if (topicId == 5) return 'Sports';
    else return '';
  }
}
