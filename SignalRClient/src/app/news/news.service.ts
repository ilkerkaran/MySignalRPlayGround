import { NewsTopicModel } from "./news-topic/news-topic-model";
import { Subject } from "rxjs";

export class NewsService {
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



}