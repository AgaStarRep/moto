import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { AuthenticationRoutes } from "./authentication.routing";
import { SigninComponent } from "./signin/signin.component";
import { SignupComponent } from "./signup/signup.component";
import { AuthGuard } from "./guards/auth-guard";
import { AuthService } from "./auth.service";
import { RoleGuard } from "./guards/role-guard";
import { TokenInterceptor } from "./token-interceptor";
import { HTTP_INTERCEPTORS } from "@angular/common/http";

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(AuthenticationRoutes),
        FormsModule,
        ReactiveFormsModule
    ],
    declarations: [SigninComponent, SignupComponent]
})
export class AuthenticationModule {
    static forRoot() {
        return {
            ngModule: AuthenticationModule,
            providers: [AuthGuard, AuthService, RoleGuard, {
                provide: HTTP_INTERCEPTORS,
                useClass: TokenInterceptor,
                multi: true
              }]
        };
    }
}
