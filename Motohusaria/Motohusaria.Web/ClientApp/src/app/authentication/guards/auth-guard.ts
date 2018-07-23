import { Injectable } from "@angular/core";
import { CanActivate } from "@angular/router";
import { Router, ActivatedRoute } from "@angular/router";
import { AuthService } from "../auth.service";

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private authService: AuthService
    ) {}

    canActivate() {
        if (!this.authService.isLoggedIn()) {
            this.router.navigateByUrl("/signin");
            return false;
        }
        return true;
    }
}
