import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { AppComponent } from './app.component';
import { CommonAppModule } from './common/commonapp.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { DatePipe } from '@angular/common';
import { MAT_DATE_LOCALE, DateAdapter, MAT_DATE_FORMATS, MatProgressBarModule } from "@angular/material";
import { MomentDateAdapter, MAT_MOMENT_DATE_FORMATS, MatMomentDateModule } from '@angular/material-moment-adapter'
import { SidebarModule } from 'ng-sidebar';

import { appRoutes } from './app.routing'

import { AdminLayoutComponent } from './layouts/admin/admin-layout.component';
import { LocalDateAdapter } from './common/local-date-adapter';
import { AuthenticationModule } from "./authentication/authentication.module";
import { RequestProgressBarModule } from './common/request-progress-bar/request-progress-bar.module';
import { RequestProgressBarComponent } from './common/request-progress-bar/request-progress-bar.component';
import { SimpleNotificationsModule } from 'angular2-notifications';
import { NotificationsComponent } from './common/notifications/notifications.component';
import { NotificationsModule } from './common/notifications/notifications.module';


export function createTranslateLoader(http: HttpClient) {
    return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
    declarations: [
        AppComponent,
        AdminLayoutComponent,
        RequestProgressBarComponent,
        NotificationsComponent,
    ],
    imports: [
        BrowserModule,
        CommonAppModule,
        RouterModule.forRoot(appRoutes),
        BrowserAnimationsModule,
        HttpClientModule,
        NgbModule.forRoot(),
        MatProgressBarModule,
        AuthenticationModule.forRoot(),
        SimpleNotificationsModule.forRoot(),
        RequestProgressBarModule.forRoot(),
        NotificationsModule.forRoot(),
        SidebarModule.forRoot(),
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: (createTranslateLoader),
                deps: [HttpClient]
            }
        }),
    ],
    providers: [DatePipe,
        { provide: MAT_DATE_LOCALE, useValue: 'pl-PL' },
        { provide: DateAdapter, useClass: LocalDateAdapter, deps: [MAT_DATE_LOCALE] },
        { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    ],
    bootstrap: [AppComponent]
}) export class AppModule { }
