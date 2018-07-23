import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule, DatePipe } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { CommonAppModule } from "../common/commonapp.module";

import { UserListComponent } from "./user-list.component";
import { UserCreateOrUpdateComponent } from "./user.component";
import { UserDeleteComponent } from "./user-delete.component";
import {
    MatDatepickerModule,
    MatNativeDateModule,
    MAT_DATE_LOCALE,
    MatInputModule,
    MatCheckboxModule,
    MatTableModule
} from "@angular/material";
import { UserFiltersComponent } from './user-filters.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
    declarations: [
        UserListComponent,
        UserCreateOrUpdateComponent,
        UserDeleteComponent,
        UserFiltersComponent
    ],
    providers: [DatePipe, { provide: MAT_DATE_LOCALE, useValue: "pl-PL" }],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        CommonAppModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatInputModule,
        MatCheckboxModule,
        MatTableModule,
        InfiniteScrollModule,
        RouterModule.forChild([
            {
                path: "user",
                component: UserListComponent,
                data: {
                    heading: 'User'
                },
                children: [
                    { path: "create", component: UserCreateOrUpdateComponent },
                    {
                        path: "edit/:id",
                        component: UserCreateOrUpdateComponent
                    },
                    { path: "delete/:id", component: UserDeleteComponent }
                ]
            }
        ])
    ]
})
export class UserModule { }
