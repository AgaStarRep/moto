
            import { NgModule } from '@angular/core';
            import { RouterModule } from '@angular/router'
            import { CommonModule,DatePipe } from '@angular/common';
            import { FormsModule,ReactiveFormsModule } from '@angular/forms';

            import {CommonAppModule} from '../common/commonapp.module';

            import { RoleListComponent } from './role-list.component';
            import { RoleCreateOrUpdateComponent } from './role.component';
            import { RoleDeleteComponent } from './role-delete.component';
			import { MatDatepickerModule, MatNativeDateModule, MAT_DATE_LOCALE, MatInputModule, MatCheckboxModule, MatTableModule, DateAdapter, MAT_DATE_FORMATS } from '@angular/material';
            import { LocalDateAdapter } from '../common/local-date-adapter';
            import { MAT_MOMENT_DATE_FORMATS } from "@angular/material-moment-adapter";
            import { RoleService } from './role.service';
            import { RoleFiltersComponent } from './role-filters.component';
            import { InfiniteScrollModule } from 'ngx-infinite-scroll';
            

            @NgModule({
                declarations: [
                    RoleListComponent,
                    RoleCreateOrUpdateComponent,
                    RoleDeleteComponent,
                    RoleFiltersComponent,
                    
                ],
                providers:[
                    RoleService,
                    DatePipe,
					{provide: MAT_DATE_LOCALE, useValue: 'pl-PL'},
                    {provide: DateAdapter,useClass: LocalDateAdapter,deps: [MAT_DATE_LOCALE]},
                    {provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
                ],
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
                            path: 'role', component: RoleListComponent, data: {heading: 'Role'},
                            children: [
                                { path: 'create', component: RoleCreateOrUpdateComponent, data: {heading: 'Role'} },
                                { path: 'edit/:id', component: RoleCreateOrUpdateComponent, data: {heading: 'Role'} },
                                { path: 'delete/:id', component: RoleDeleteComponent, data: {heading: 'Role'} }]
                        },
                    ])
                ]
            })
            export class RoleModule {

            }
            
