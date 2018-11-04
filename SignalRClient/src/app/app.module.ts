import { AuthGuard } from './shared/auth.guard';
import { AuthService } from './shared/auth.service';
import { EnvironmentUrlService } from './shared/environment-url.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NewsComponent } from './news/news.component';
import { NewsTopicComponent } from './news/news-topic/news-topic.component';
import { NewsService } from './news/news.service';
import { NavigationComponent } from './navigation/navigation.component';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NewsComponent,
    NewsTopicComponent,
    NavigationComponent,
    LoginComponent
  ],
  imports: [BrowserModule, AppRoutingModule, FormsModule, HttpClientModule],
  providers: [NewsService, EnvironmentUrlService, AuthService, AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule {}
