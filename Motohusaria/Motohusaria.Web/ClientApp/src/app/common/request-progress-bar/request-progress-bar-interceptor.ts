import { Injectable } from "@angular/core";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse
} from "@angular/common/http";
import { RequestProgressBarService } from "./request-progress-bar.service";

import { Observable } from "rxjs";
import {finalize} from 'rxjs/operators';

@Injectable()
export class RequestProgressBarInterceptor implements HttpInterceptor {
    constructor(public service: RequestProgressBarService) {}

    currentRequests = 0;

    intercept(request: HttpRequest<any>,httpHandler: HttpHandler): Observable<HttpEvent<any>> {
        this.currentRequests ++;
        if(this.currentRequests == 1){
            this.service.setVisbility(true);
        }
        return httpHandler.handle(request).pipe(finalize(() => {
            this.currentRequests--;
                if (this.currentRequests == 0)
                    this.service.setVisbility(false);
        }))
    }
}
