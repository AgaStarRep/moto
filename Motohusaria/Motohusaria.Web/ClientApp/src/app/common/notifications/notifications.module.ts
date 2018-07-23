import {NgModule} from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http'
import { NotificationsInterceptor } from './notifications-interceptor.service';

@NgModule({

})
export class NotificationsModule{
    static forRoot(){
        return {
            ngModule: NotificationsModule,
            providers: [{
                    provide: HTTP_INTERCEPTORS,
                    useClass: NotificationsInterceptor,
                    multi: true
            }]
        }
    }
}
