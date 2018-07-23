import { Component } from '@angular/core';
import { Options } from 'angular2-notifications';

@Component({
    selector: 'notifications',
    styles: [
        `.notifications {
        position: relative;
        z-index: 1005;
        }`
    ],
    template: '<div class="notifications"><simple-notifications [options]="options"></simple-notifications></div>'
})
export class NotificationsComponent {
    options: Options = {
        showProgressBar: true,
        position: ['top', 'right'],
        timeOut: 0,
        preventDuplicates: true,
    }
}