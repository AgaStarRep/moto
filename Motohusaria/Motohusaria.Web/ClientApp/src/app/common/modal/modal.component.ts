import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'modal',
    styleUrls: ["./modal.component.css"],
    templateUrl: './modal.component.html',
})
export class ModalComponent {
    @Input()
    header: string;
    @Input()
    show: boolean;
    @Input()
    large: boolean;

    @Output()
    modalClose = new EventEmitter();

    close() {
        this.modalClose.emit();
    }
}

