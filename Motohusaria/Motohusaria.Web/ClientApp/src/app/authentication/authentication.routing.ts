import { Routes } from "@angular/router";

import { SigninComponent } from "./signin/signin.component";
import { SignupComponent } from "./signup/signup.component";
import { RoleGuard } from "./guards/role-guard";

export const AuthenticationRoutes: Routes = [
    {
        path: "signin",
        component: SigninComponent,
        canActivate: [RoleGuard],
        data: {opo: "ok"}
    },
    {
        path: "signup",
        component: SignupComponent
    }
];
