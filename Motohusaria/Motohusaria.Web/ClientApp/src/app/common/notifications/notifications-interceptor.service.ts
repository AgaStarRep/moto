import { Injectable } from "@angular/core";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse
} from "@angular/common/http";
import { NotificationsService } from "angular2-notifications";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { Notification } from "./notification";

@Injectable()
export class NotificationsInterceptor implements HttpInterceptor {
  constructor(public service: NotificationsService) {}

  intercept(
    request: HttpRequest<any>,
    httpHandler: HttpHandler
  ): Observable<HttpEvent<any>> {
    return httpHandler.handle(request).pipe(tap(event => {
      if (event instanceof HttpResponse) {
        if (event.body && event.body.notifications && (<Notification[]>event.body.notifications).length > 0) {
            (<Notification[]>event.body.notifications).forEach(f => {
                this.notify(f);
            });
        }
      }
    }));
  }

  private notify(notification: Notification) {
    switch (notification.type) {
      case 0: //Success
        this.service.success(notification.title,notification.content);
        break;
      case 1: //Info
        this.service.info(notification.title,notification.content);
        break;
      case 2: //Warning
        this.service.warn(notification.title,notification.content);
        break;
      case 3: //Alert
        this.service.alert(notification.title,notification.content);
        break;
      case 4: //Error
        this.service.error(notification.title,notification.content);
        break;
      default:
        break;
    }
  }
}
