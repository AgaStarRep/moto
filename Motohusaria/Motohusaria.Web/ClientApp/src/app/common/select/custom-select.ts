import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { Component, ElementRef, Input, OnDestroy, forwardRef, Optional, Self, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR, NgModel, NgControl, NgForm, FormGroupDirective } from '@angular/forms';
import { MatFormFieldControl, CanUpdateErrorState } from '@angular/material';
import { Subject ,  Observable ,  Subscription } from 'rxjs';
import { distinctUntilChanged, debounceTime } from 'rxjs/operators';

@Component({
    selector: 'custom-select',
    templateUrl: './custom-select.html',
    providers: [
        { provide: MatFormFieldControl, useExisting: CustomSelect }
    ],
    host: {
        '[class.floating]': 'shouldLabelFloat',
        '[id]': 'id',
        '[attr.aria-describedby]': 'describedBy',
    }
})

export class CustomSelect implements MatFormFieldControl<string[] | string>, OnDestroy, ControlValueAccessor {
    static nextId = 0;

    typeHeadSubscription: Subscription;

    private selected: string[] | string;

    stateChanges = new Subject<void>();

    focused = false;

    @Output() onSearched = new EventEmitter<string>();
    typeahead: EventEmitter<string>;

    @Input() get useClientFilter() { return this._useClientFilter; }
    set useClientFilter(value) { this._useClientFilter = value; this.setTypeahead(); }
    _useClientFilter: boolean = false;

    @Input() get searchDelay() { return this._searchDelay }
    set searchDelay(value) { this._searchDelay = value; this.setTypeahead(); }
    _searchDelay = 200;

    @Input() get searchMinLength() { return this._searchMinLength }
    set searchMinLength(value) { this._searchMinLength = value; this.setTypeahead(); }
    _searchMinLength = 0;

    @Input() get errorState() {
        var submited = (this.parentForm && this.parentForm.submitted) || (this.parentFormGroup && this.parentFormGroup.submitted);
        if (submited && !this.ngControl.touched) {
            //jeżeli bylo submitowane bez dotkoniecia to oznaczam jako dotkniete zeby bledy walidacji sie pojawily
            this.propagateTouched();
            this.ngControl.control.markAsDirty();
            this.stateChanges.next();
        }
        var val = this.ngControl.errors !== null && (this.ngControl.touched);
        return val;
    }

    controlType = 'my-select';

    @Input() multiple: boolean;

    @Input() options = [];

    @Input() form: FormGroup;

    propagateChange = (_: any) => { };
    propagateTouched = () => { };

    get empty() {
        var val = !this.selected || this.selected.length == 0;
        return val;
    }

    get shouldLabelFloat() { return this.focused || !this.empty; }

    id = `my-tel-input-${CustomSelect.nextId++}`;

    describedBy = '';

    @Input()
    get placeholder() { return this._placeholder; }
    set placeholder(plh) {
        this._placeholder = plh;
        this.stateChanges.next();
    }
    private _placeholder: string;

    @Input()
    get required() { return this._required; }
    set required(req) {
        this._required = coerceBooleanProperty(req);
        this.stateChanges.next();
    }
    private _required = false;

    @Input()
    get disabled() { return this._disabled; }
    set disabled(dis) {
        this._disabled = coerceBooleanProperty(dis);
        this.stateChanges.next();
    }
    private _disabled = false;

    @Input()
    get value(): string[] | string | null {
        if (!this.selected || this.selected.length === 0) {
            return null;
        }
        return this.selected;
    }
    set value(val: string[] | string | null) {
        if (!val) {
            val = this.multiple ? [] : "";
        }
        this.selected = val;
        this.propagateChange(this.value);
        this.stateChanges.next();
    }

    constructor(fb: FormBuilder, private fm: FocusMonitor, private elRef: ElementRef, public ngControl: NgControl, @Optional() private parentForm: NgForm,
        @Optional() private parentFormGroup: FormGroupDirective) {

        ngControl.valueAccessor = this;
        fm.monitor(elRef.nativeElement, true).subscribe((origin) => {
            this.focused = !!origin;
            this.stateChanges.next();
        });
        this.setTypeahead();
    }

    setTypeahead() {
        if (this.useClientFilter) {
            this.typeahead = null;
            return;
        }
        if (this.typeHeadSubscription) {
            this.typeHeadSubscription.unsubscribe();
        }
        this.typeahead = new EventEmitter<string>();
        this.typeHeadSubscription = this.typeahead
            .pipe(distinctUntilChanged(),
                debounceTime(this.searchDelay))
            .subscribe(s => {
                if (this.searchMinLength == 0)
                    this.onSearched.next(s);

                else if (s && s.length >= this.searchMinLength) {
                    this.onSearched.next(s);
                }
            });
    }

    writeValue(obj: any): void {
        this.value = obj;
    }

    setDisabledState?(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    registerOnChange(fn: any): void {
        this.propagateChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.propagateTouched = fn;
    }

    public fireTouched(arg: any) {
        this.propagateTouched();
    }

    ngOnDestroy() {
        this.stateChanges.complete();
        this.fm.stopMonitoring(this.elRef.nativeElement);
    }

    setDescribedByIds(ids: string[]) {
        this.describedBy = ids.join(' ');
    }

    onContainerClick(event: MouseEvent) {
        if ((event.target as Element).tagName.toLowerCase() != 'input') {
            this.elRef.nativeElement.querySelector('input').focus();
        }
    }
}