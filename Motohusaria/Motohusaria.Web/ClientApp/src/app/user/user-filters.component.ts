
import { Component, OnInit, OnDestroy, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { UserService } from './user.service';
import { UserListModel } from './user-list-model';
import { ServiceResponse } from '../common/service-response-model'
import { LookupModel } from '../common/lookup-model';


import { formatDateToMomentJsObj, formatMomentJSDateToString } from '../common/form-utils';
@Component({
    selector: 'user-filters',
    templateUrl: './user-filters.component.html'
})
export class UserFiltersComponent implements OnInit, OnDestroy {
    constructor(private route: ActivatedRoute, private router: Router, private service: UserService,  private formBuilder: FormBuilder, private datePipe: DatePipe ) { 
        this.entityForm = this.formBuilder.group({
            login : [null,null],

        });
    }

    @Output() filter = new EventEmitter<UserListModel>();

    entityForm: FormGroup;
    formSubscription: Subscription;
    
    

    get filters() {
        if (!this.entityForm) {
            return null;
        }
        const value = Object.assign({}, this.entityForm.value);
        //pola z datą powinny być zwracane w string YYYY-MM-DD, wykorzystaj funkcje formatMomentJSDateToString

        return value as UserListModel;
    }

    setFilters(value: any) {
        if (!this.entityForm) {
            return;
        }
        //pola z datą powinny być zwracane jako obiekt momentjs, wykorzystaj funkcje formatDateToMomentJsObj

        this.entityForm.patchValue(value, { emitEvent: false });
    }

    ngOnInit() {
		this.getFormOptions();
        
        this.formSubscription = this.entityForm.valueChanges
            .pipe(debounceTime(500)
            ,distinctUntilChanged())    
            .subscribe(next => {
                this.onSubmit();
            });
    }

    async getFormOptions() {
}


    async onSubmit() {
		 this.filter.emit(this.filters);
    }

    ngOnDestroy(): void {
        if(this.formSubscription){
            this.formSubscription.unsubscribe();            
        }
    }
}
