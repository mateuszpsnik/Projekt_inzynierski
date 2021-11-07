import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';

import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { MusiciansComponent } from './musicians/musicians.component';
import { HomeFormComponent } from './home-form/home-form.component';
import { ReviewsWidgetComponent } from './reviews-widget/reviews-widget.component';
import { SignupWidgetComponent } from './signup-widget/signup-widget.component';
import { FooterComponent } from './footer/footer.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './angular-material.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MusicianComponent } from './musician/musician.component';
import { ReviewsListComponent } from './reviews-list/reviews-list.component';
import { MeetingComponent } from './meeting/meeting.component';
import { ReportComponent } from './report/report.component';
import { MenuComponent } from './menu/menu.component';
import { MeetingsComponent } from './meetings/meetings.component';
import { MeetingsListComponent } from './meetings-list/meetings-list.component';
import { MyDatePipe } from './meetings-list/date.pipe';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    MusiciansComponent,
    MusicianComponent,
    HomeFormComponent,
    ReviewsWidgetComponent,
    ReviewsListComponent,
    SignupWidgetComponent,
    FooterComponent,
    MeetingComponent,
    ReportComponent,
    MenuComponent,
    MeetingsComponent,
    MeetingsListComponent,
    MyDatePipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ApiAuthorizationModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    NgbModule,
    BrowserAnimationsModule,
    AngularMaterialModule,
    FontAwesomeModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthorizeInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})

export class AppModule { }
