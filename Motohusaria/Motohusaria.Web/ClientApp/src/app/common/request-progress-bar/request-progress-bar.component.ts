import { Component, OnInit, OnDestroy } from '@angular/core'
import { RequestProgressBarService } from './request-progress-bar.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'request-progress-bar',
    styleUrls: ['./request-progress-bar.component.scss'],
    template: `<div class="app-progress-bar" *ngIf="show"><mat-progress-bar mode="indeterminate"></mat-progress-bar></div>`
})
export class RequestProgressBarComponent {
    constructor(private service: RequestProgressBarService){
    }

    public show: boolean = false;
    private subscription: Subscription;

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }

    ngOnInit(): void {
        this.subscription = this.service.visable.subscribe(next => {
            this.show = next;
        });
    }
}