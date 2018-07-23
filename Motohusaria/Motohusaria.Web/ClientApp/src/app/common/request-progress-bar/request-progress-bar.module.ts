import { NgModule } from "@angular/core"
import { HTTP_INTERCEPTORS } from '@angular/common/http'
import { RequestProgressBarComponent } from './request-progress-bar.component';
import { RequestProgressBarService } from './request-progress-bar.service';
import { RequestProgressBarInterceptor } from './request-progress-bar-interceptor';

@NgModule({
})
export class RequestProgressBarModule {
    static forRoot(){
        return {
            ngModule: RequestProgressBarModule,
            providers: [RequestProgressBarService, {
                provide: HTTP_INTERCEPTORS,
                useClass: RequestProgressBarInterceptor,
                multi: true
              }]
        }
    }
}