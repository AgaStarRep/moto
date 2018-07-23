import { Routes } from '@angular/router'
import { AdminLayoutComponent } from './layouts/admin/admin-layout.component';
import { AuthGuard } from './authentication/guards/auth-guard';

export const appRoutes : Routes = [
    {
        path: "",
        component: AdminLayoutComponent,
        canActivate: [AuthGuard],
        children: [
            {path: '', loadChildren: "./role/role.module#RoleModule"},
            {path: '', loadChildren: "./user/user.module#UserModule"},
        ]
    }
]