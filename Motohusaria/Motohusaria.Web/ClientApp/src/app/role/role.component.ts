
import { Component, OnInit, ViewChild } from '@angular/core';
import { RoleService } from './role.service';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';

import { RoleModel } from './role-model';
import { ServiceResponse } from '../common/service-response-model'
import { LookupModel } from '../common/lookup-model';
import { UserService } from '../user/user.service';


@Component({
    templateUrl: './role.component.html'
})
export class RoleCreateOrUpdateComponent implements OnInit {
    constructor(private route: ActivatedRoute, private router: Router, private service: RoleService, private userService: UserService, private formBuilder: FormBuilder, private datePipe: DatePipe) { }

    isEdit: boolean = false;
    triedSubmit: boolean = false;
    header: string;
    isVisable: boolean = false;
    entityForm: FormGroup;
    errors: string[];
    roleUsersOptions: LookupModel[];



    ngOnInit() {
        this.route.paramMap.subscribe(next => this.loadEntityForm(next.get('id')));
        this.getFormOptions();

    }

    async getFormOptions() {
        this.getRoleUsersOptions();
    }
    getRoleUsersOptions(search?: string | null) {
        this.userService.getOptions(search).then((data) => this.roleUsersOptions = data);
    }

    async loadEntityForm(id: string | null) {
        var entity: RoleModel;
        if (id) {
            this.header = 'Edytuj';
            entity = await this.service.getById(id);
            if (!entity) {
                this.close(true);
                return;
            }
            this.isEdit = true;
        }
        else {
            this.header = 'Dodaj';
            entity = <RoleModel>{};
        }
        this.entityForm = this.formBuilder.group({
            name: [entity.name, Validators.compose([Validators.required, Validators.maxLength(128)])],
            roleUsersSelectedIds: [entity.roleUsersSelectedIds, null],
            id: [entity.id ? entity.id : null, null],

        });

        this.isVisable = true;
    }

    async onSubmit() {
        this.triedSubmit = true;
        if (!this.entityForm.valid) {
            return;
        }
        this.entityForm.markAsDirty();
        this.entityForm.markAsTouched();
        const value = this.entityForm.value as any;

        var result: ServiceResponse;
        if (this.isEdit) {
            result = await this.service.update(value);
        }
        else {
            result = await this.service.add(value);
        }
        if (result.success) {
            this.close();
            return;
        }
    }

    close(clearFromHistory = false) {
        var extras = { queryParams: this.route.snapshot.queryParams }
        if (clearFromHistory) {
            extras['replaceUrl'] = true;
        }
        this.router.navigate(['role'], extras);
    }
}
