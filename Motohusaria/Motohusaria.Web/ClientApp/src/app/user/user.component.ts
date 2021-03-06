import { Component, OnInit, ViewChild } from "@angular/core";
import { UserService } from "./user.service";
import { ActivatedRoute, Router, ParamMap } from "@angular/router";
import {
    FormControl,
    FormGroup,
    FormBuilder,
    Validators
} from "@angular/forms";
import { DatePipe } from "@angular/common";

import { UserModel } from "./user-model";
import {
    ServiceResponse
} from "../common/service-response-model";
import { LookupModel } from "../common/lookup-model";
import { RoleService } from "../role/role.service";
import { matchingPasswords } from "../common/validators/compare-password.validator";

@Component({
    templateUrl: "./user.component.html"
})
export class UserCreateOrUpdateComponent implements OnInit {
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private service: UserService,
        private roleService: RoleService,
        private formBuilder: FormBuilder,
        private datePipe: DatePipe
    ) {}

    isEdit: boolean = false;
    triedSubmit: boolean = false;
    header: string;
    isVisable: boolean = false;
    entityForm: FormGroup;
    errors: string[];
    userRolesOptions: LookupModel[];

    ngOnInit() {
        this.route.paramMap.subscribe(next =>
            this.loadEntityForm(next.get("id"))
        );
        this.getFormOptions();
    }

    async getFormOptions() {
        this.getUserRolesOptions();
    }
    getUserRolesOptions(search?: string | null) {
        this.roleService
            .getOptions(search)
            .then(data => (this.userRolesOptions = data));
    }

    async loadEntityForm(id: string | null) {
        var entity: UserModel;
        if (id) {
            this.header = "Edytuj";
            entity = await this.service.getById(id);
            if (!entity) {
                this.close(true);
                return;
            }
            this.isEdit = true;
        } else {
            this.header = "Dodaj";
            entity = <UserModel>{};
        }
        this.entityForm = this.formBuilder.group({
            login: [
                entity.login,
                Validators.compose([
                    Validators.required,
                    Validators.maxLength(128)
                ])
            ],
            password: [
                "******",
                Validators.compose([
                    Validators.required,
                    Validators.maxLength(32)
                ])
            ],
            passwordConfirm: [
                "******",
                Validators.compose([
                    Validators.required
                ])
            ],
            userRolesSelectedIds: [entity.userRolesSelectedIds, null],
            id: [entity.id ? entity.id : null, null]
        }, {validator: matchingPasswords('password','passwordConfirm')});

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
        } else {
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
        this.router.navigate(["user"], extras);
    }
}
