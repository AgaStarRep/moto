import { Injectable } from "@angular/core";
import { CanActivate } from "@angular/router";
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { AuthService } from "../auth.service";

@Injectable()
export class RoleGuard implements CanActivate {
    constructor(
        private router: Router,
        private authService: AuthService
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const data = route.data;
        if(data && data.roles && (<string[]>data.roles).length > 0){
            const userRoles = this.authService.Roles.concat();
            const anyRoleMatched = (<string[]>data.roles).some(s => userRoles.indexOf(s) > -1);
            return anyRoleMatched
        }
        return true;
    }
}
